using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Researcher.Api.Common.Models;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Models;
using Researcher.Application.Requests.Graphs.Commands;
using Researcher.Application.Requests.Graphs.Queries;
using Wolverine;

namespace Researcher.Api.Common.Extensions.EndpointsExtensions;

/// <summary>
/// Расширения маршрутов для работы с графами.
/// </summary>
public static class GraphEndpoints
{
    /// <summary>
    /// Добавляет эндпоинты для работы с графами.
    /// </summary>
    /// <param name="routes">Маршрутизатор для регистрации эндпоинтов.</param>
    public static void MapGraphEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/graphs").WithTags("Graphs");

        // Создание графа
        group.MapPost("/", async (
                [FromBody] CreateGraphCommand cmd,
                [FromServices] IMessageBus bus) =>
            {
                var created = await bus.InvokeAsync<GraphDto>(cmd);
                return Results.Created($"/graphs/{created.Id}", created);
            })
            .WithName("CreateGraph")
            .WithSummary("Создаёт новый граф в проекте")
            .Accepts<CreateGraphCommand>(MediaTypeNames.Application.Json)
            .Produces<GraphDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Удаление графа по Id
        group.MapDelete("/{graphId:guid}", async (
                [FromRoute] Guid graphId,
                [FromServices] IMessageBus bus) =>
            {
                await bus.InvokeAsync<object>(new DeleteGraphCommand(graphId));
                return Results.NoContent();
            })
            .WithName("DeleteGraph")
            .WithSummary("Удаляет граф по идентификатору")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Массовое обновление графа с метаданными, узлами и ребрами
        group.MapPut("/{graphId:guid}/full", async (
                [FromRoute] Guid graphId,
                [FromBody] UpdateFullGraphRequest updateRequest,
                [FromServices] IMessageBus bus) =>
            {
                var cmd = new UpdateFullGraphCommand(
                    graphId,
                    updateRequest.NewTitle,
                    updateRequest.NewDescription,
                    updateRequest.Nodes,
                    updateRequest.Edges);

                var updated = await bus.InvokeAsync<FullGraphDto>(cmd);
                return Results.Ok(updated);
            })
            .WithName("UpdateFullGraph")
            .WithSummary("Массовое обновление графа: метаданные, узлы, ребра")
            .Accepts<UpdateFullGraphRequest>(MediaTypeNames.Application.Json)
            .Produces<FullGraphDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение полного графа с узлами и ребрами
        group.MapGet("/{graphId:guid}/full", async (
                [FromRoute] Guid graphId,
                [FromServices] IMessageBus bus) =>
            {
                var fullGraph = await bus.InvokeAsync<FullGraphDto?>(new GetFullGraphQuery(graphId));
                return fullGraph is null ? Results.NotFound() : Results.Ok(fullGraph);
            })
            .WithName("GetFullGraph")
            .WithSummary("Получает полный граф с метаданными, узлами и ребрами")
            .Produces<FullGraphDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение деталей графа (без узлов и ребер)
        group.MapGet("/{graphId:guid}", async (
                [FromRoute] Guid graphId,
                [FromServices] IMessageBus bus) =>
            {
                var graph = await bus.InvokeAsync<GraphDto?>(new GetGraphByIdQuery(graphId));
                return graph is null ? Results.NotFound() : Results.Ok(graph);
            })
            .WithName("GetGraphById")
            .WithSummary("Получает детали графа без узлов и ребер")
            .Produces<GraphDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение списка графов проекта с пагинацией
        group.MapGet("/", async (
                [FromQuery, Required] Guid projectId,
                [FromQuery] int? page,
                [FromQuery] int? pageSize,
                [FromServices] IMessageBus bus) =>
            {
                var query = new ListGraphsByProjectQuery(projectId, page, pageSize);
                var result = await bus.InvokeAsync<PaginatedResult<GraphDto>>(query);
                return Results.Ok(result);
            })
            .WithName("ListGraphsByProject")
            .WithSummary("Получает список графов проекта с пагинацией")
            .Produces<PaginatedResult<GraphDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}