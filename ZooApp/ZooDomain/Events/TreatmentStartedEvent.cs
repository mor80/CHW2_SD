using ZooDomain.ValueObjects;

namespace ZooDomain.Events;

public sealed record TreatmentStartedEvent(AnimalId AnimalId, string Diagnosis, DateTime StartedAt) : IDomainEvent;