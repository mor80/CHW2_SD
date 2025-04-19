using ZooDomain.ValueObjects;

namespace ZooDomain.Events;

public sealed record FeedingTimeEvent(AnimalId AnimalId, DateTime FeedingAt) : IDomainEvent;