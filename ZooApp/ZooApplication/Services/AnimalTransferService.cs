using ZooApplication.Abstractions;
using ZooDomain.ValueObjects;

namespace ZooApplication.Services;

public class AnimalTransferService
{
    private readonly IAnimalRepository _animals;
    private readonly IEnclosureRepository _enclosures;
    private readonly IEventDispatcher _events;

    public AnimalTransferService(IAnimalRepository animals,
        IEnclosureRepository enclosures,
        IEventDispatcher events)
    {
        _animals = animals;
        _enclosures = enclosures;
        _events = events;
    }

    public async Task TransferAsync(AnimalId animalId, EnclosureId targetId, CancellationToken ct = default)
    {
        var animal = await _animals.GetAsync(animalId, ct) ??
                     throw new KeyNotFoundException("Животное не найдено.");
        var current = await _enclosures.GetAsync(animal.EnclosureId, ct) ??
                      throw new KeyNotFoundException("Текущий вольер не найден.");
        var target = await _enclosures.GetAsync(targetId, ct) ??
                     throw new KeyNotFoundException("Целевой вольер не найден.");

        current.RemoveAnimal(animal);
        target.AddAnimal(animal);
        animal.MoveTo(target.Id);

        await _enclosures.SaveChangesAsync(ct);
        await _animals.SaveChangesAsync(ct);

        await _events.DispatchAsync(animal.DomainEvents, ct);
        animal.ClearEvents();
    }
}