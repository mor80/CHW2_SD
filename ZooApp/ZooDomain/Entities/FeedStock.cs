using ZooDomain.Enums;
using ZooDomain.Events;
using ZooDomain.ValueObjects;

namespace ZooDomain.Entities;

public class FeedStock
{
    private readonly List<IDomainEvent> _events = new();

    public Guid Id { get; } = Guid.NewGuid();
    public DietType FoodType { get; }
    public Quantity Amount { get; private set; }
    public int LowLevelThreshold { get; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _events.AsReadOnly();

    public FeedStock(DietType type, Quantity amount, int lowLevelThreshold = 50)
    {
        FoodType = type;
        Amount = amount;
        LowLevelThreshold = lowLevelThreshold;
    }

    public void Withdraw(int portion)
    {
        Amount -= portion;
        if (Amount.IsNegative)
            throw new InvalidOperationException("Недостаточно корма на складе.");

        if (Amount.Value <= LowLevelThreshold)
            _events.Add(new FeedStockLowEvent(FoodType, Amount.Value));
    }

    public void Refill(int amount) => Amount = new Quantity(Amount.Value + amount);

    public void ClearEvents() => _events.Clear();
}