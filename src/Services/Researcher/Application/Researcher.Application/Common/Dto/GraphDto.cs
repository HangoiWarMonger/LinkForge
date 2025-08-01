namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO для графа (Graph).\**
/// </summary>
public record GraphDto(
    Guid Id,
    string Title,
    string? Description,
    Guid ProjectId,
    DateTimeOffset CreatedAtUtc
); // Title, Description, ProjectId :contentReference[oaicite:7]{index=7}