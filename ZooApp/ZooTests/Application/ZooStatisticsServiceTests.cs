using ZooApplication.Services;
using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooInfrastructure.Repositories;

namespace ZooTests.Application;

public class ZooStatisticsServiceTests
{
    [Fact]
    public async Task GetStats_ReturnsCorrectNumbers()
    {
        var animals = new InMemoryAnimalRepository();
        var enclosures = new InMemoryEnclosureRepository();

        var pen1 = new Enclosure(EnclosureType.Herbivore, 2);
        var pen2 = new Enclosure(EnclosureType.Predator, 2);
        await enclosures.AddAsync(pen1);
        await enclosures.AddAsync(pen2);

        var giraffe = new Animal("Giraffe", "Melman", new System.DateTime(2015, 5, 1),
            Gender.Male, DietType.Vegetables, pen1.Id);
        pen1.AddAnimal(giraffe);
        await animals.AddAsync(giraffe);

        var svc = new ZooStatisticsService(animals, enclosures);
        var stats = await svc.GetStatsAsync();

        Assert.Equal(1, stats.AnimalCount);
        Assert.Equal(2, stats.EnclosuresTotal);
        Assert.Equal(2, stats.EnclosuresFree);
    }
}