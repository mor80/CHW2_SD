using ZooDomain.ValueObjects;

namespace ZooDomain.Events;

public sealed record AnimalMovedEvent(AnimalId AnimalId, EnclosureId From, EnclosureId To) : IDomainEvent;