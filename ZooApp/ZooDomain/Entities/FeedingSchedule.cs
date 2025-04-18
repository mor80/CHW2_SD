using ZooDomain.Enums;
using ZooDomain.ValueObjects;

namespace ZooDomain.Entities;

public class FeedingSchedule
{
    public Guid Id { get; } = Guid.NewGuid();
    public AnimalId AnimalId { get; }
    public TimeOnly FeedingTime { get; private set; }
    public DietType Food { get; private set; }

    public FeedingSchedule(AnimalId animalId, TimeOnly feedingTime, DietType food)
    {
        AnimalId = animalId;
        FeedingTime = feedingTime;
        Food = food;
    }

    public void Change(TimeOnly time, DietType food)
    {
        FeedingTime = time;
        Food = food;
    }
}