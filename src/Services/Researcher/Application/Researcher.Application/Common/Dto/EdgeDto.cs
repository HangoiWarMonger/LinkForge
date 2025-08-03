namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO связи между узлами.
/// </summary>
/// <param name="Id">Идентификатор связи.</param>
/// <param name="Type">Тип связи.</param>
/// <param name="Description">Описание связи.</param>
/// <param name="FromNodeId">Идентификатор узла-источника.</param>
/// <param name="ToNodeId">Идентификатор узла-приёмника.</param>
/// <param name="CreatedAtUtc">Дата создания.</param>
public record EdgeDto(
    Guid Id,
    string Type,
    string? Description,
    Guid FromNodeId,
    Guid ToNodeId,
    DateTimeOffset CreatedAtUtc
);