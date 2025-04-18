using ZooDomain.Enums;
using ZooDomain.ValueObjects;

namespace ZooDomain.Entities;

public class Enclosure
{
    private readonly HashSet<AnimalId> _animalIds = new();

    private const int CleanlinessMax = 100;
    private const int CleanlinessMin = 0;

    public EnclosureId Id { get; }
    public EnclosureType Type { get; }
    public int MaxCapacity { get; }
    public int Cleanliness { get; private set; } = CleanlinessMax;

    public IReadOnlyCollection<AnimalId> AnimalIds => _animalIds;

    public Enclosure(EnclosureType type, int maxCapacity)
    {
        Id = EnclosureId.New();
        Type = type;
        MaxCapacity = maxCapacity;
    }

    public void AddAnimal(Animal animal)
    {
        if (_animalIds.Count >= MaxCapacity)
            throw new InvalidOperationException("Вольер переполнен.");

        if (!IsCompatible(animal))
            throw new InvalidOperationException("Несовместимые типы животных.");

        if (Cleanliness < 20)
            throw new InvalidOperationException("Вольер слишком грязный — нужно убраться.");

        _animalIds.Add(animal.Id);
    }

    public void RemoveAnimal(Animal animal) => _animalIds.Remove(animal.Id);

    public void Clean() => Cleanliness = CleanlinessMax;

    public void DecreaseCleanliness(int delta = 5)
    {
        Cleanliness = Math.Max(CleanlinessMin, Cleanliness - delta);
    }

    private bool IsCompatible(Animal animal)
    {
        return Type switch
        {
            EnclosureType.Predator => animal.FavoriteFood == DietType.Meat || animal.FavoriteFood == DietType.Fish,
            EnclosureType.Herbivore => animal.FavoriteFood == DietType.Vegetables,
            _ => true
        };
    }
}