using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Researcher.Api.Common.Models;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Models;
using Researcher.Application.Requests.Projects.Commands;
using Researcher.Application.Requests.Projects.Queries;
using Wolverine;

namespace Researcher.Api.Common.Extensions.EndpointsExtensions;

/// <summary>
/// Расширения маршрутов для работы с проектами.
/// </summary>
public static class ProjectEndpoints
{
    /// <summary>
    /// Добавляет эндпоинты для работы с проектами.
    /// </summary>
    /// <param name="routes">Маршрутизатор для регистрации эндпоинтов.</param>
    public static void MapProjectEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/projects").WithTags("Projects");

        // Создание проекта
        group.MapPost("/", async (
                [FromBody] CreateProjectCommand cmd,
                [FromServices] IMessageBus bus) =>
            {
                var created = await bus.InvokeAsync<ProjectDto>(cmd);
                return Results.Created($"/projects/{created.Id}", created);
            })
            .WithName("CreateProject")
            .WithSummary("Создаёт новый проект")
            .Accepts<CreateProjectCommand>(MediaTypeNames.Application.Json)
            .Produces<ProjectDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Удаление проекта по Id
        group.MapDelete("/{projectId:guid}", async (
                [FromRoute] Guid projectId,
                [FromServices] IMessageBus bus) =>
            {
                await bus.InvokeAsync<object>(new DeleteProjectCommand(projectId));
                return Results.NoContent();
            })
            .WithName("DeleteProject")
            .WithSummary("Удаляет проект по идентификатору")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Обновление проекта
        group.MapPut("/{projectId:guid}", async (
                [FromRoute] Guid projectId,
                [FromBody] UpdateProjectRequest updateRequest,
                [FromServices] IMessageBus bus) =>
            {
                var cmd = new UpdateProjectCommand(
                    projectId,
                    updateRequest.NewName,
                    updateRequest.NewDescription);
                var updated = await bus.InvokeAsync<ProjectDto>(cmd);
                return Results.Ok(updated);
            })
            .WithName("UpdateProject")
            .WithSummary("Обновляет проект по идентификатору")
            .Accepts<UpdateProjectRequest>(MediaTypeNames.Application.Json)
            .Produces<ProjectDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение проекта по Id
        group.MapGet("/{projectId:guid}", async (
                [FromRoute] Guid projectId,
                [FromServices] IMessageBus bus) =>
            {
                var project = await bus.InvokeAsync<ProjectDto?>(new GetProjectByIdQuery(projectId));
                return project is null ? Results.NotFound() : Results.Ok(project);
            })
            .WithName("GetProjectById")
            .WithSummary("Получает проект по идентификатору")
            .Produces<ProjectDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение списка проектов с сортировкой и пагинацией
        group.MapGet("/", async (
                [FromServices] IMessageBus bus,
                [FromQuery] string? orderBy,
                [FromQuery] SortDirection sortDirection = SortDirection.Ascending,
                [FromQuery] int? page = null,
                [FromQuery] int? pageSize = null) =>
            {
                var query = new ListProjectsQuery(orderBy, sortDirection, page, pageSize);
                var result = await bus.InvokeAsync<PaginatedResult<ProjectDto>>(query);
                return Results.Ok(result);
            })
            .WithName("ListProjects")
            .WithSummary("Получает список проектов с пагинацией и сортировкой")
            .Produces<PaginatedResult<ProjectDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}