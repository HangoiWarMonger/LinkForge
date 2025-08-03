using Researcher.Domain.ValueObjects;

namespace Researcher.Application.Common.Dto;

/// <summary>
/// DTO задачи проекта.
/// </summary>
/// <param name="Id">Идентификатор задачи.</param>
/// <param name="Title">Заголовок задачи.</param>
/// <param name="Description">Описание задачи.</param>
/// <param name="Status">Статус задачи.</param>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="ParentId">Идентификатор родительской задачи.</param>
/// <param name="CreatedAtUtc">Дата создания.</param>
public record TaskItemDto(
    Guid Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    Guid ProjectId,
    Guid? ParentId,
    DateTimeOffset CreatedAtUtc
);