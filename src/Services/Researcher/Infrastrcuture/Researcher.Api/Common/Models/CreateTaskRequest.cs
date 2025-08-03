using System.ComponentModel.DataAnnotations;
using Researcher.Domain.ValueObjects;

namespace Researcher.Api.Common.Models;

/// <summary>
/// Запрос для создания новой задачи.
/// </summary>
public record CreateTaskRequest(
    [property: Required] Guid ProjectId,
    [property: Required] string Title,
    [property: Required] TaskItemStatus Status,
    Guid? ParentId,
    string? Description);