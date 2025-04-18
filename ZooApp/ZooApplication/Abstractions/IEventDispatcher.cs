using ZooDomain.Events;

namespace ZooApplication.Abstractions;

public interface IEventDispatcher
{
    Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default);
}