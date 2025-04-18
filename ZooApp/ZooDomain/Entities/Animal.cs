using ZooDomain.Enums;
using ZooDomain.Events;
using ZooDomain.ValueObjects;

namespace ZooDomain.Entities;

public class Animal
{
    private readonly List<IDomainEvent> _events = new();

    public AnimalId Id { get; }
    public string Name { get; private set; }
    public string Species { get; }
    public DateTime BirthDate { get; }
    public Gender Gender { get; }
    public DietType FavoriteFood { get; }
    public HealthStatus Status { get; set; }
    public EnclosureId EnclosureId { get; private set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    public Animal(string species, string name, DateTime birthDate, Gender gender,
        DietType favoriteFood, EnclosureId enclosureId)
    {
        Id = AnimalId.New();
        Species = species;
        Name = name;
        BirthDate = birthDate;
        Gender = gender;
        FavoriteFood = favoriteFood;
        Status = HealthStatus.Healthy;
        EnclosureId = enclosureId;
    }

    public void Feed(DietType food, DateTime at)
    {
        if (food != FavoriteFood && FavoriteFood != DietType.Mixed)
            throw new InvalidOperationException($"{Name} отказывается есть {food}!");

        _events.Add(new FeedingTimeEvent(Id, at));
    }

    public void Treat() => Status = HealthStatus.Healthy;

    public void MoveTo(EnclosureId target)
    {
        var from = EnclosureId;
        EnclosureId = target;
        _events.Add(new AnimalMovedEvent(Id, from, target));
    }

    public void ClearEvents() => _events.Clear();
}