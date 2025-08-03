namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO узла графа.
/// </summary>
/// <param name="Id">Идентификатор узла.</param>
/// <param name="Title">Заголовок узла.</param>
/// <param name="Description">Описание узла.</param>
/// <param name="Type">Тип узла.</param>
/// <param name="Position">Позиция узла.</param>
/// <param name="GraphId">Идентификатор графа.</param>
/// <param name="CreatedAtUtc">Дата создания.</param>
public record NodeDto(
    Guid Id,
    string Title,
    string? Description,
    string Type,
    PositionDto Position,
    Guid GraphId,
    DateTimeOffset CreatedAtUtc
);