using ZooApplication.Abstractions;
using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.ValueObjects;

namespace ZooApplication.Services;

public class HealthService
{
    private readonly IAnimalRepository _animals;
    private readonly ITreatmentRepository _cases;
    private readonly IEventDispatcher _events;

    public HealthService(IAnimalRepository animals,
        ITreatmentRepository cases,
        IEventDispatcher events)
    {
        _animals = animals;
        _cases = cases;
        _events = events;
    }

    public async Task<Guid> StartTreatmentAsync(AnimalId id, string diagnosis, CancellationToken ct = default)
    {
        var animal = await _animals.GetAsync(id, ct) ?? throw new KeyNotFoundException();
        animal.Status = HealthStatus.Sick;

        var tr = new TreatmentCase(id, diagnosis);
        await _cases.AddAsync(tr, ct);

        await _events.DispatchAsync(tr.DomainEvents, ct);
        tr.ClearEvents();
        await _animals.SaveChangesAsync(ct);
        await _cases.SaveChangesAsync(ct);
        return tr.Id;
    }

    public async Task FinishTreatmentAsync(Guid caseId, CancellationToken ct = default)
    {
        var tr = await _cases.GetAsync(caseId, ct) ?? throw new KeyNotFoundException();
        tr.Finish();

        var animal = await _animals.GetAsync(tr.AnimalId, ct)!;
        animal.Treat();

        await _events.DispatchAsync(tr.DomainEvents, ct);
        tr.ClearEvents();
        await _animals.SaveChangesAsync(ct);
        await _cases.SaveChangesAsync(ct);
    }
}