namespace Researcher.Application.Common.Dto;

/// <summary>
/// Полное представление графа с узлами и связями для фронтенда.
/// </summary>
/// <param name="Id">Идентификатор графа.</param>
/// <param name="Title">Название графа.</param>
/// <param name="Description">Описание графа.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="CreatedAtUtc">Дата создания графа.</param>
/// <param name="Nodes">Список узлов графа.</param>
/// <param name="Edges">Список связей графа.</param>
public record FullGraphDto(
    Guid Id,
    string Title,
    string? Description,
    Guid ProjectId,
    DateTimeOffset CreatedAtUtc,
    IReadOnlyList<NodeDto> Nodes,
    IReadOnlyList<EdgeDto> Edges
);