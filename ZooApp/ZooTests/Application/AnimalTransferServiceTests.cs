using ZooApplication.Services;
using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.ValueObjects;
using ZooInfrastructure.Repositories;

namespace ZooTests.Application;

public class AnimalTransferServiceTests
{
    private static Animal CreateLion(EnclosureId eid) =>
        new("Panther leo", "Alex", new System.DateTime(2016, 4, 2),
            Gender.Male, DietType.Meat, eid);

    [Fact]
    public async Task Transfer_MovesAnimalAndPublishesEvent()
    {
        var animals = new InMemoryAnimalRepository();
        var enclosures = new InMemoryEnclosureRepository();
        var dispatcher = new FakeDispatcher();

        var from = new Enclosure(EnclosureType.Predator, 2);
        var to = new Enclosure(EnclosureType.Predator, 2);
        await enclosures.AddAsync(from);
        await enclosures.AddAsync(to);

        var lion = CreateLion(from.Id);
        await animals.AddAsync(lion);
        from.AddAnimal(lion);

        var svc = new AnimalTransferService(animals, enclosures, dispatcher);

        await svc.TransferAsync(lion.Id, to.Id);

        Assert.Contains(lion.Id, to.AnimalIds);
        Assert.DoesNotContain(lion.Id, from.AnimalIds);
        Assert.Single(dispatcher.Published);
    }
}