using ZooApplication.Services;
using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.ValueObjects;
using ZooInfrastructure.Repositories;

namespace ZooTests.Application;

public class HealthServiceTests
{
    private static Animal CreateElephant(EnclosureId eid) =>
        new("Elephant", "Dumbo", new DateTime(2018, 7, 1),
            Gender.Male, DietType.Vegetables, eid);

    [Fact]
    public async Task StartTreatment_SetsAnimalSickAndPublishesEvent()
    {
        var animals = new InMemoryAnimalRepository();
        var casesRepo = new InMemoryTreatmentRepository();
        var events = new FakeDispatcher();

        var pen = new Enclosure(EnclosureType.Herbivore, 2);
        var dumbo = CreateElephant(pen.Id);
        await animals.AddAsync(dumbo);

        var health = new HealthService(animals, casesRepo, events);


        var caseId = await health.StartTreatmentAsync(dumbo.Id, "Flu");


        var tr = await casesRepo.GetAsync(caseId);
        Assert.NotNull(tr);
        Assert.Equal(HealthStatus.Sick, dumbo.Status);
        Assert.Single(events.Published);
    }

    [Fact]
    public async Task FinishTreatment_HealsAnimalAndPublishesEvent()
    {
        var animals = new InMemoryAnimalRepository();
        var casesRepo = new InMemoryTreatmentRepository();
        var events = new FakeDispatcher();

        var pen = new Enclosure(EnclosureType.Herbivore, 2);
        var dumbo = CreateElephant(pen.Id);
        await animals.AddAsync(dumbo);

        var health = new HealthService(animals, casesRepo, events);
        var caseId = await health.StartTreatmentAsync(dumbo.Id, "Infection");

        events.Published.Clear();


        await health.FinishTreatmentAsync(caseId);


        var tr = await casesRepo.GetAsync(caseId);
        Assert.NotNull(tr!.FinishedAt);
        Assert.Equal(HealthStatus.Healthy, dumbo.Status);
        Assert.Single(events.Published);
    }
}