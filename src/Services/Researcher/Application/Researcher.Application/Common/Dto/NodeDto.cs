namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO для узла (Node).\**
/// </summary>
public record NodeDto(
    Guid Id,
    string Title,
    string? Description,
    string Type,
    PositionDto Position,
    Guid GraphId,
    DateTimeOffset CreatedAtUtc
);