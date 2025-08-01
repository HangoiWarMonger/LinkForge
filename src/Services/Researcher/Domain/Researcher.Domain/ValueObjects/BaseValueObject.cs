namespace Researcher.Domain.ValueObjects;

/// <summary>
/// Базовый класс для Value Object.
/// Сравнение происходит по элементам, возвращаемым методом GetEqualityComponents.
/// </summary>
public abstract class BaseValueObject : IEquatable<BaseValueObject>
{
    /// <summary>
    /// Возвращает последовательность компонентов, используемых для сравнения объекта.
    /// </summary>
    protected abstract IEnumerable<object?> GetEqualityComponents();

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
            return false;

        return Equals(obj as BaseValueObject);
    }

    /// <inheritdoc />
    public bool Equals(BaseValueObject? other)
    {
        if (other is null)
            return false;

        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        // Используем комбинацию хешей компонентов с XOR и умножением
        unchecked
        {
            int hash = 17;
            foreach (var component in GetEqualityComponents())
            {
                hash = hash * 23 + (component?.GetHashCode() ?? 0);
            }

            return hash;
        }
    }

    /// <summary>
    /// Проверяет равенство двух объектов-значений.
    /// </summary>
    /// <param name="left">Левый объект-значение.</param>
    /// <param name="right">Правый объект-значение.</param>
    /// <returns>True, если объекты равны; иначе false.</returns>
    public static bool operator ==(BaseValueObject? left, BaseValueObject? right)
    {
        if (ReferenceEquals(left, right))
            return true;

        if (left is null || right is null)
            return false;

        return left.Equals(right);
    }

    /// <summary>
    /// Проверяет неравенство двух объектов-значений.
    /// </summary>
    /// <param name="left">Левый объект-значение.</param>
    /// <param name="right">Правый объект-значение.</param>
    /// <returns>True, если объекты не равны; иначе false.</returns>
    public static bool operator !=(BaseValueObject? left, BaseValueObject? right)
    {
        return !(left == right);
    }
}