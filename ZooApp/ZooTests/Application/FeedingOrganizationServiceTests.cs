using ZooApplication.Services;
using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.Events;
using ZooDomain.ValueObjects;
using ZooInfrastructure.Repositories;

namespace ZooTests.Application;

public class FeedingOrganizationServiceTests
{
    private static Animal CreateGiraffe(EnclosureId eid) =>
        new("Giraffe", "Melman", new DateTime(2015, 5, 1),
            Gender.Male, DietType.Vegetables, eid);

    [Fact]
    public async Task FeedDueAnimals_WithdrawsStockAndPublishesEvents()
    {
        var animalsRepo = new InMemoryAnimalRepository();
        var schedulesRepo = new InMemoryFeedingScheduleRepository();
        var stockRepo = new InMemoryFeedStockRepository();
        var dispatcher = new FakeDispatcher();

        var enc = new Enclosure(EnclosureType.Herbivore, 3);
        var giraffe = CreateGiraffe(enc.Id);
        await animalsRepo.AddAsync(giraffe);

        var time = new TimeOnly(12, 0);
        await schedulesRepo.AddAsync(new FeedingSchedule(giraffe.Id, time, DietType.Vegetables));
        
        var stock = new FeedStock(DietType.Vegetables, new Quantity(10));
        await stockRepo.AddAsync(stock);

        var service = new FeedingOrganizationService(animalsRepo, schedulesRepo, stockRepo, dispatcher);
        var now = DateTime.Today.Add(time.ToTimeSpan());
        await service.FeedDueAnimalsAsync(now);

        Assert.Equal(9, stock.Amount.Value);
        Assert.Equal(2, dispatcher.Published.Count);

        Assert.Contains(dispatcher.Published, e => e is FeedingTimeEvent);
        Assert.Contains(dispatcher.Published, e => e is FeedStockLowEvent low && low.Remainder == 9);
    }
}