namespace ZooApplication.Abstractions;

public interface IRepository<T, TId>
{
    Task<T?> GetAsync(TId id, CancellationToken ct = default);
    Task<IEnumerable<T>> ListAsync(CancellationToken ct = default);
    Task AddAsync(T entity, CancellationToken ct = default);
    Task RemoveAsync(T entity, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}