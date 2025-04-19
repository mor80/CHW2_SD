using ZooDomain.ValueObjects;

namespace ZooDomain.Events;

public sealed record TreatmentFinishedEvent(AnimalId AnimalId, DateTime FinishedAt) : IDomainEvent;