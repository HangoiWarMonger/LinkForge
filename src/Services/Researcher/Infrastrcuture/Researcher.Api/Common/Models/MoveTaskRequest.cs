namespace Researcher.Api.Common.Models;

/// <summary>
/// Запрос на перемещение задачи под нового родителя или в корень.
/// </summary>
public record MoveTaskRequest(Guid? NewParentId);