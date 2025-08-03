using System.ComponentModel.DataAnnotations;
using Researcher.Domain.ValueObjects;

namespace Researcher.Api.Common.Models;

/// <summary>
/// Запрос для изменения статуса задачи.
/// </summary>
public record ChangeTaskStatusRequest(
    [property: Required] TaskItemStatus NewStatus);