using ZooApplication.Abstractions;
using ZooDomain.Events;

namespace ZooTests.Application;

public class FakeDispatcher : IEventDispatcher
{
    public readonly List<IDomainEvent> Published = new();

    public Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        Published.AddRange(events);
        return Task.CompletedTask;
    }
}