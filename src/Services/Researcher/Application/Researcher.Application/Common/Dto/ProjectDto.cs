namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO проекта.
/// </summary>
/// <param name="Id">Идентификатор проекта.</param>
/// <param name="Name">Название проекта.</param>
/// <param name="Description">Описание проекта.</param>
/// <param name="CreatedAtUtc">Дата создания.</param>
public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset CreatedAtUtc
);