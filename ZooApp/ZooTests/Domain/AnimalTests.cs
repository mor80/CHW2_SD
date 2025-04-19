using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.ValueObjects;

namespace ZooTests.Domain;

public class AnimalTests
{
    private static Animal Create() =>
        new("Lion", "Simba", DateTime.UtcNow.AddYears(-3), Gender.Male,
            DietType.Meat, EnclosureId.New());

    [Fact]
    public void Feed_WithWrongFood_ShouldThrow()
    {
        var a = Create();
        Assert.Throws<InvalidOperationException>(() => a.Feed(DietType.Vegetables, DateTime.UtcNow));
    }

    [Fact]
    public void Feed_ShouldAddDomainEvent()
    {
        var a = Create();
        a.Feed(DietType.Meat, DateTime.UtcNow);
        Assert.Single(a.DomainEvents);
    }
}