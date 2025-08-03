using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Researcher.Api.Common.Models;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Models;
using Researcher.Application.Requests.Tasks.Commands;
using Researcher.Application.Requests.Tasks.Queries;
using Researcher.Domain.ValueObjects;
using Wolverine;

namespace Researcher.Api.Common.Extensions.EndpointsExtensions;

/// <summary>
/// Расширения маршрутов для работы с задачами.
/// </summary>
public static class TaskEndpoints
{
    /// <summary>
    /// Добавляет эндпоинты для работы с задачами.
    /// </summary>
    /// <param name="routes">Маршрутизатор для регистрации эндпоинтов.</param>
    public static void MapTaskEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/tasks").WithTags("Tasks");

        // Смена статуса задачи
        group.MapPatch("/{taskItemId:guid}/status", async (
                [FromRoute] Guid taskItemId,
                [FromBody] ChangeTaskStatusRequest request,
                [FromServices] IMessageBus bus) =>
            {
                var cmd = new ChangeTaskStatusCommand(taskItemId, request.NewStatus);
                var updated = await bus.InvokeAsync<TaskItemDto>(cmd);
                return Results.Ok(updated);
            })
            .WithName("ChangeTaskStatus")
            .WithSummary("Смена статуса задачи")
            .Accepts<ChangeTaskStatusRequest>(MediaTypeNames.Application.Json)
            .Produces<TaskItemDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Создание задачи
        group.MapPost("/", async (
                [FromBody] CreateTaskRequest request,
                [FromServices] IMessageBus bus) =>
            {
                var cmd = new CreateTaskCommand(
                    request.ProjectId,
                    request.Title,
                    request.Status,
                    request.ParentId,
                    request.Description);
                var created = await bus.InvokeAsync<TaskItemDto>(cmd);
                return Results.Created($"/tasks/{created.Id}", created);
            })
            .WithName("CreateTask")
            .WithSummary("Создаёт новую задачу")
            .Accepts<CreateTaskRequest>(MediaTypeNames.Application.Json)
            .Produces<TaskItemDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Удаление задачи
        group.MapDelete("/{taskItemId:guid}", async (
                [FromRoute] Guid taskItemId,
                [FromServices] IMessageBus bus) =>
            {
                await bus.InvokeAsync<object>(new DeleteTaskCommand(taskItemId));
                return Results.NoContent();
            })
            .WithName("DeleteTask")
            .WithSummary("Удаляет задачу по идентификатору")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Перемещение задачи под нового родителя
        group.MapPatch("/{taskItemId:guid}/move", async (
                [FromRoute] Guid taskItemId,
                [FromBody] MoveTaskRequest request,
                [FromServices] IMessageBus bus) =>
            {
                var cmd = new MoveTaskCommand(taskItemId, request.NewParentId);
                var updated = await bus.InvokeAsync<TaskItemDto>(cmd);
                return Results.Ok(updated);
            })
            .WithName("MoveTask")
            .WithSummary("Перемещает задачу под нового родителя (или в корень)")
            .Accepts<MoveTaskRequest>(MediaTypeNames.Application.Json)
            .Produces<TaskItemDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Полное обновление задачи
        group.MapPut("/{taskItemId:guid}", async (
                [FromRoute] Guid taskItemId,
                [FromBody] UpdateTaskRequest request,
                [FromServices] IMessageBus bus) =>
            {
                var cmd = new UpdateTaskCommand(
                    taskItemId,
                    request.NewTitle,
                    request.NewStatus,
                    request.NewParentId,
                    request.NewDescription);
                var updated = await bus.InvokeAsync<TaskItemDto>(cmd);
                return Results.Ok(updated);
            })
            .WithName("UpdateTask")
            .WithSummary("Полное обновление задачи")
            .Accepts<UpdateTaskRequest>(MediaTypeNames.Application.Json)
            .Produces<TaskItemDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение задачи по Id
        group.MapGet("/{taskItemId:guid}", async (
                [FromRoute] Guid taskItemId,
                [FromServices] IMessageBus bus) =>
            {
                var task = await bus.InvokeAsync<TaskItemDto?>(new GetTaskByIdQuery(taskItemId));
                return task is null ? Results.NotFound() : Results.Ok(task);
            })
            .WithName("GetTaskById")
            .WithSummary("Получает задачу по идентификатору")
            .Produces<TaskItemDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение плоского списка всех задач проекта (с ParentId для дерева)
        group.MapGet("/tree", async (
                [FromQuery, Required] Guid projectId,
                [FromServices] IMessageBus bus) =>
            {
                var query = new GetTaskTreeByProjectQuery(projectId);
                var tasks = await bus.InvokeAsync<IReadOnlyList<TaskItemDto>>(query);
                return Results.Ok(tasks);
            })
            .WithName("GetTaskTreeByProject")
            .WithSummary("Получает плоский список всех задач проекта с ParentId для построения дерева")
            .Produces<IReadOnlyList<TaskItemDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Список задач проекта с фильтрами, сортировкой и пагинацией
        group.MapGet("/", async (
                [FromQuery, Required] Guid projectId,
                [FromServices] IMessageBus bus,
                [FromQuery] TaskItemStatus? status,
                [FromQuery] string? orderBy,
                [FromQuery] SortDirection sortDirection = SortDirection.Ascending,
                [FromQuery] int? page = null,
                [FromQuery] int? pageSize = null) =>
            {
                var query = new ListTasksByProjectQuery(
                    projectId,
                    status,
                    orderBy,
                    sortDirection,
                    page,
                    pageSize);
                var result = await bus.InvokeAsync<PaginatedResult<TaskItemDto>>(query);
                return Results.Ok(result);
            })
            .WithName("ListTasksByProject")
            .WithSummary("Получает список задач проекта с фильтрацией, сортировкой и пагинацией")
            .Produces<PaginatedResult<TaskItemDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}