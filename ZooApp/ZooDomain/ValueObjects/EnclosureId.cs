namespace ZooDomain.ValueObjects;

public readonly record struct EnclosureId(Guid Value)
{
    public static EnclosureId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString();
}