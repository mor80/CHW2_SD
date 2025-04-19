using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.ValueObjects;

namespace ZooTests.Domain;

public class EnclosureTests
{
    private static Animal CreatePredator() =>
        new("Panther leo", "Simba", new DateTime(2019, 3, 14),
            Gender.Male, DietType.Meat, EnclosureId.New());

    private static Animal CreateHerbivore() =>
        new("Elephant maximus", "Dumbo", new DateTime(2018, 7, 1),
            Gender.Male, DietType.Vegetables, EnclosureId.New());

    [Fact]
    public void AddAnimal_Compatible_Succeeds()
    {
        var enclosure = new Enclosure(EnclosureType.Predator, 2);
        var lion = CreatePredator();

        enclosure.AddAnimal(lion);

        Assert.Contains(lion.Id, enclosure.AnimalIds);
    }

    [Fact]
    public void AddAnimal_Incompatible_Throws()
    {
        var enclosure = new Enclosure(EnclosureType.Predator, 2);
        var herbivore = CreateHerbivore();

        Assert.Throws<InvalidOperationException>(() => enclosure.AddAnimal(herbivore));
    }

    [Fact]
    public void AddAnimal_OverCapacity_Throws()
    {
        var enclosure = new Enclosure(EnclosureType.Predator, 1);
        var lion1 = CreatePredator();
        var lion2 = CreatePredator();

        enclosure.AddAnimal(lion1);
        Assert.Throws<InvalidOperationException>(() => enclosure.AddAnimal(lion2));
    }

    [Fact]
    public void AddAnimal_WhenDirty_Throws()
    {
        var enclosure = new Enclosure(EnclosureType.Predator, 1);
        var lion = CreatePredator();

        for (int i = 0; i < 20; i++)
            enclosure.DecreaseCleanliness(5); // 100 -> 0

        Assert.Throws<InvalidOperationException>(() => enclosure.AddAnimal(lion));
    }
}