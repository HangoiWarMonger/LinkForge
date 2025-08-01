namespace Researcher.Domain.ValueObjects;

/// <summary>
/// Объект-значение, представляющий позицию на плоскости с координатами X и Y.
/// </summary>
public sealed class Position : BaseValueObject
{
    /// <summary>
    /// Координата X.
    /// </summary>
    public double X { get; }

    /// <summary>
    /// Координата Y.
    /// </summary>
    public double Y { get; }

    /// <summary>
    /// Создаёт позицию с заданными координатами.
    /// </summary>
    /// <param name="x">Координата X.</param>
    /// <param name="y">Координата Y.</param>
    public Position(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <inheritdoc />
    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}