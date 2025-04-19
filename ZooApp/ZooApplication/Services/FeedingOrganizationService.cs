using ZooApplication.Abstractions;

namespace ZooApplication.Services;

public class FeedingOrganizationService
{
    private readonly IAnimalRepository _animals;
    private readonly IFeedingScheduleRepository _schedules;
    private readonly IFeedStockRepository _stocks;
    private readonly IEventDispatcher _events;
    private const int PortionSize = 1;

    public FeedingOrganizationService(IAnimalRepository animals,
        IFeedingScheduleRepository schedules,
        IFeedStockRepository stocks,
        IEventDispatcher events)
    {
        _animals = animals;
        _schedules = schedules;
        _stocks    = stocks;
        _events   = events;
    }

    public async Task FeedDueAnimalsAsync(DateTime now, CancellationToken ct = default)
    {
        var due = (await _schedules.ListAsync(ct))
            .Where(s => s.FeedingTime == TimeOnly.FromDateTime(now));

        foreach (var schedule in due)
        {
            var animal = await _animals.GetAsync(schedule.AnimalId, ct);
            if (animal is null) continue;

            var stock = (await _stocks.ListAsync(ct))
                        .FirstOrDefault(s => s.FoodType == schedule.Food)
                        ?? throw new InvalidOperationException("Нет склада для такого корма.");

            stock.Withdraw(PortionSize);
            animal.Feed(schedule.Food, now);

            await _events.DispatchAsync(animal.DomainEvents.Concat(stock.DomainEvents), ct);
            animal.ClearEvents(); stock.ClearEvents();
        }

        await _animals.SaveChangesAsync(ct);
        await _stocks.SaveChangesAsync(ct);
    }
}