using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Researcher.Api.Common.Models;
using Researcher.Application.Common.Dto;
using Researcher.Application.Common.Models;
using Researcher.Application.Requests.Documents.Commands;
using Researcher.Application.Requests.Documents.Queries;
using Wolverine;

namespace Researcher.Api.Common.Extensions.EndpointsExtensions;

/// <summary>
/// Расширения маршрутов для работы с документами.
/// </summary>
public static class DocumentEndpoints
{
    /// <summary>
    /// Добавляет эндпоинты для работы с документами.
    /// </summary>
    public static void MapDocumentEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/documents").WithTags("Documents");

        // Удаление документа по Id
        group.MapDelete("/{documentId:guid}", async (
                [FromRoute] Guid documentId,
                [FromServices] IMessageBus bus) =>
            {
                await bus.InvokeAsync<object>(new DeleteDocumentCommand(documentId));
                return Results.NoContent();
            })
            .WithName("DeleteDocument")
            .WithSummary("Удаляет документ по идентификатору")
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Создание нового документа
        group.MapPost("/", async (
                [FromBody] CreateDocumentCommand cmd,
                [FromServices] IMessageBus bus) =>
            {
                var result = await bus.InvokeAsync<DocumentDto>(cmd);
                return Results.Created($"/documents/{result.Id}", result);
            })
            .WithName("CreateDocument")
            .WithSummary("Создаёт новый документ в проекте")
            .Accepts<CreateDocumentCommand>(MediaTypeNames.Application.Json)
            .Produces<DocumentDto>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Обновление документа
        group.MapPut("/{documentId:guid}", async (
                [FromRoute] Guid documentId,
                [FromBody] UpdateDocumentRequest updateRequest,
                [FromServices] IMessageBus bus) =>
            {
                var cmd = new UpdateDocumentCommand(
                    documentId,
                    updateRequest.NewTitle,
                    updateRequest.NewBodyMarkdown,
                    updateRequest.NewIsInternal);
                var updated = await bus.InvokeAsync<DocumentDto>(cmd);
                return Results.Ok(updated);
            })
            .WithName("UpdateDocument")
            .WithSummary("Обновляет документ по идентификатору")
            .Accepts<UpdateDocumentRequest>(MediaTypeNames.Application.Json)
            .Produces<DocumentDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Получение документа по Id
        group.MapGet("/{documentId:guid}", async (
                [FromRoute] Guid documentId,
                [FromServices] IMessageBus bus) =>
            {
                var doc = await bus.InvokeAsync<DocumentDto?>(new GetDocumentByIdQuery(documentId));
                return doc is null ? Results.NotFound() : Results.Ok(doc);
            })
            .WithName("GetDocumentById")
            .WithSummary("Получает документ по идентификатору")
            .Produces<DocumentDto>()
            .Produces(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Список документов проекта с фильтрами
        group.MapGet("/", async (
                [FromQuery, Required] Guid projectId,
                [FromServices] IMessageBus bus,
                [FromQuery] bool? isInternal,
                [FromQuery] string? orderBy,
                [FromQuery] SortDirection sortDirection = SortDirection.Ascending,
                [FromQuery] int? page = null,
                [FromQuery] int? pageSize = null) =>
            {
                var query = new ListDocumentsByProjectQuery(
                    projectId,
                    isInternal,
                    orderBy,
                    sortDirection,
                    page,
                    pageSize);
                var result = await bus.InvokeAsync<PaginatedResult<DocumentDto>>(query);
                return Results.Ok(result);
            })
            .WithName("ListDocumentsByProject")
            .WithSummary("Получает список документов проекта с фильтрацией, сортировкой и пагинацией")
            .Produces<PaginatedResult<DocumentDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);

        // Поиск документов по тексту и проекту
        group.MapGet("/search", async (
                [FromQuery] Guid? projectId,
                [FromQuery, Required] string searchText,
                [FromServices] IMessageBus bus) =>
            {
                var query = new SearchDocumentsQuery(projectId, searchText);
                var result = await bus.InvokeAsync<PaginatedResult<DocumentDto>>(query);
                return Results.Ok(result);
            })
            .WithName("SearchDocuments")
            .WithSummary("Ищет документы по тексту с опциональным фильтром по проекту")
            .Produces<PaginatedResult<DocumentDto>>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError);
    }
}