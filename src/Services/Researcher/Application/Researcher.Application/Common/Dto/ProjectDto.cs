namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO для проекта (Project).
/// </summary>
public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    DateTimeOffset CreatedAtUtc
);