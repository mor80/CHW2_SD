using ZooDomain.Enums;

namespace ZooDomain.Events;

public sealed record FeedStockLowEvent(DietType FoodType, int Remainder) : IDomainEvent;