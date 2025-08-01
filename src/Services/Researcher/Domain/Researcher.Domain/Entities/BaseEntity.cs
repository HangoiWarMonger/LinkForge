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
    public DateTimeOffset CreatedAtUtc { get; protected init; }

    /// <summary>
    /// Инициализирует базовую сущность с заданным уникальным идентификатором и временем создания.
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности.</param>
    /// <exception cref="ArgumentException">Если идентификатор является значением по умолчанию.</exception>
    protected BaseEntity(Guid id)
    {
        Guard.Against.Default(id);
        Id = id;
        CreatedAtUtc = DateTimeOffset.UtcNow;
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

    /// <inheritdoc />
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

    /// <summary>
    /// Определяет оператор равенства для сравнения двух сущностей по их идентификаторам.
    /// </summary>
    /// <param name="left">Левая сущность для сравнения.</param>
    /// <param name="right">Правая сущность для сравнения.</param>
    /// <returns>True, если сущности равны; иначе false.</returns>
    public static bool operator ==(BaseEntity? left, BaseEntity? right)
    {
        if (left is null && right is null)
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Определяет оператор неравенства для сравнения двух сущностей по их идентификаторам.
    /// </summary>
    /// <param name="left">Левая сущность для сравнения.</param>
    /// <param name="right">Правая сущность для сравнения.</param>
    /// <returns>True, если сущности не равны; иначе false.</returns>
    public static bool operator !=(BaseEntity? left, BaseEntity? right)
    {
        return !(left == right);
    }

    #endregion
}