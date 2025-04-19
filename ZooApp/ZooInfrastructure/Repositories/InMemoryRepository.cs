using ZooApplication.Abstractions;

namespace ZooInfrastructure.Repositories;

public abstract class InMemoryRepository<T, TId> : IRepository<T, TId>
    where T : class
{
    protected readonly Dictionary<TId, T> Store = new();

    public virtual Task AddAsync(T entity, CancellationToken ct = default)
    {
        var id = (TId)entity!.GetType().GetProperty("Id")!.GetValue(entity)!;
        Store[id] = entity;
        return Task.CompletedTask;
    }

    public Task<T?> GetAsync(TId id, CancellationToken ct = default)
        => Task.FromResult(Store.TryGetValue(id, out var e) ? e : null);

    public Task<IEnumerable<T>> ListAsync(CancellationToken ct = default)
        => Task.FromResult(Store.Values.AsEnumerable());

    public Task RemoveAsync(T entity, CancellationToken ct = default)
    {
        var id = (TId)entity!.GetType().GetProperty("Id")!.GetValue(entity)!;
        Store.Remove(id);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync(CancellationToken ct = default) => Task.CompletedTask;
}