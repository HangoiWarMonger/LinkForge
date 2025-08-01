using Ardalis.GuardClauses;
using Researcher.Domain.Events;

namespace Researcher.Domain.Entities;

/// <summary>
/// Базовая сущность с уникальным идентификатором и временем создания.
/// Поддерживает доменные события и сравнение по идентификатору.
/// </summary>
public abstract class BaseEntity : IEquatable<BaseEntity>
{
    private readonly List<IDomainEvent> _events = new();

    /// <summary>
    /// Уникальный идентификатор сущности.
    /// </summary>
    public Guid Id { get; protected init; }

    /// <summary>
    /// Время создания сущности в UTC.
    /// </summary>
    public DateTime CreatedAtUtc { get; protected init; }

    protected BaseEntity(Guid id)
    {
        Guard.Against.Default(id);
        Id = id;
        CreatedAtUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Конструктор для ORM или сериализации.
    /// </summary>
    protected BaseEntity()
    {
    }

    /// <summary>
    /// Коллекция доменных событий, связанных с сущностью.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> Events => _events.AsReadOnly();

    /// <summary>
    /// Добавляет доменное событие к сущности.
    /// </summary>
    /// <param name="event">Доменные событие для добавления.</param>
    protected void AddEvent(IDomainEvent @event)
    {
        Guard.Against.Null(@event, nameof(@event));
        _events.Add(@event);
    }

    #region Equality

    public override bool Equals(object? obj)
    {
        if (obj is not BaseEntity other)
            return false;

        return Equals(other);
    }

    /// <inheritdoc />
    public bool Equals(BaseEntity? other)
    {
        if (other is null)
            return false;

        // Сравниваем только по Id, игнорируем тип и ссылки
        if (Id == Guid.Empty || other.Id == Guid.Empty)
            return false;

        return Id == other.Id;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(BaseEntity? left, BaseEntity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    public static bool operator !=(BaseEntity? left, BaseEntity? right)
    {
        return !(left == right);
    }

    #endregion
}