namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO графа.
/// </summary>
/// <param name="Id">Идентификатор графа.</param>
/// <param name="Title">Название графа.</param>
/// <param name="Description">Описание графа.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="CreatedAtUtc">Дата создания.</param>
public record GraphDto(
    Guid Id,
    string Title,
    string? Description,
    Guid ProjectId,
    DateTimeOffset CreatedAtUtc
);