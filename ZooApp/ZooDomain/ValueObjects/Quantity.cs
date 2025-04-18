namespace ZooDomain.ValueObjects;

public readonly record struct Quantity(int Value)
{
    public static Quantity operator -(Quantity q, int value) => new(q.Value - value);
    public bool IsNegative => Value < 0;
    public override string ToString() => Value.ToString();
}