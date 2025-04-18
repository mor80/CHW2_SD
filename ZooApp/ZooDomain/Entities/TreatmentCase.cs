using ZooDomain.Events;
using ZooDomain.ValueObjects;

namespace ZooDomain.Entities;

public class TreatmentCase
{
    public Guid Id { get; } = Guid.NewGuid();
    public AnimalId AnimalId { get; }
    public string Diagnosis { get; }
    public DateTime StartedAt { get; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; private set; }

    private readonly List<IDomainEvent> _events = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events;

    public TreatmentCase(AnimalId animalId, string diagnosis)
    {
        AnimalId = animalId;
        Diagnosis = diagnosis;
        _events.Add(new TreatmentStartedEvent(animalId, diagnosis, StartedAt));
    }

    public void Finish()
    {
        if (FinishedAt is not null)
            throw new InvalidOperationException("Кейс уже закрыт.");

        FinishedAt = DateTime.UtcNow;
        _events.Add(new TreatmentFinishedEvent(AnimalId, FinishedAt.Value));
    }

    public void ClearEvents() => _events.Clear();
}