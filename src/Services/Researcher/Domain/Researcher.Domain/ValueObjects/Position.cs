namespace Researcher.Domain.ValueObjects;

public sealed class Position : BaseValueObject
{
    public double X { get; }
    public double Y { get; }

    public Position(double x, double y)
    {
        X = x;
        Y = y;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}
