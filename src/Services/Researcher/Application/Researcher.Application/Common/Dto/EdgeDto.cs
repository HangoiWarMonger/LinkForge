namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO для связи (Edge).\**
/// </summary>
public record EdgeDto(
    Guid Id,
    string Type,
    string? Description,
    Guid FromNodeId,
    Guid ToNodeId,
    DateTimeOffset CreatedAtUtc
);