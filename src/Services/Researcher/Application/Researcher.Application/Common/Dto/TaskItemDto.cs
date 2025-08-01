using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO для задачи (TaskItem).\**
/// </summary>
public record TaskItemDto(
    Guid Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    Guid ProjectId,
    Guid? ParentId,
    DateTimeOffset CreatedAtUtc
);