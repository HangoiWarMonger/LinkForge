namespace Researcher.Domain.Events;

/// <summary>
/// Интерфейс доменного события.
/// Доменные события описывают значимые изменения в состоянии сущности,
/// которые могут быть обработаны отдельными обработчиками (например, для интеграции, логирования, триггеров).
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Время создания события в UTC.
    /// </summary>
    DateTimeOffset OccurredOnUtc { get; }

    /// <summary>
    /// Опционально: идентификатор источника события (например, Id сущности, с которой связано событие).
    /// </summary>
    Guid? SourceId { get; }
}