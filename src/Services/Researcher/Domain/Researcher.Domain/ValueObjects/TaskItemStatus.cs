namespace Researcher.Domain.ValueObjects;

/// <summary>
/// Статус задачи.
/// </summary>
public enum TaskItemStatus
{
    /// <summary>
    /// Неопределённый статус.
    /// </summary>
    Undefined = 0,

    /// <summary>
    /// Задача в состоянии "К выполнению".
    /// </summary>
    Todo = 1,

    /// <summary>
    /// Задача выполнена.
    /// </summary>
    Done = 2
}