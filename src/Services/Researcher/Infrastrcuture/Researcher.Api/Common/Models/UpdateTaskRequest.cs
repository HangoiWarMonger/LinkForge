using System.ComponentModel.DataAnnotations;
using Researcher.Domain.ValueObjects;

namespace Researcher.Api.Common.Models;

/// <summary>
/// Запрос на обновление задачи.
/// </summary>
public record UpdateTaskRequest(
    [property: Required] string NewTitle,
    [property: Required] TaskItemStatus NewStatus,
    Guid? NewParentId,
    string? NewDescription);