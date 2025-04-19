namespace ZooDomain.ValueObjects;

public readonly record struct AnimalId(Guid Value)
{
    public static AnimalId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}