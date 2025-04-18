using Microsoft.Extensions.Logging;
using ZooApplication.Abstractions;
using ZooDomain.Events;

namespace ZooInfrastructure.Events;

public class InMemoryEventDispatcher : IEventDispatcher
{
    private readonly ILogger<InMemoryEventDispatcher> _logger;

    public InMemoryEventDispatcher(ILogger<InMemoryEventDispatcher> logger) => _logger = logger;

    public Task DispatchAsync(IEnumerable<IDomainEvent> events, CancellationToken ct = default)
    {
        foreach (var e in events)
            _logger.LogInformation("Domain event: {Event}", e);

        return Task.CompletedTask;
    }
}