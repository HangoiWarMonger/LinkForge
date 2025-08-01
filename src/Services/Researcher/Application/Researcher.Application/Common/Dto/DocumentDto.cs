namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO для документа (Document).\**
/// </summary>
public record DocumentDto(
    Guid Id,
    string Title,
    string BodyMarkdown,
    bool IsInternal,
    DateTimeOffset UpdatedAt,
    Guid ProjectId,
    DateTimeOffset CreatedAtUtc
); 