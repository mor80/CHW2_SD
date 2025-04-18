using ZooApplication.Abstractions;
using ZooDomain.Entities;

namespace ZooInfrastructure.Repositories;

public sealed class InMemoryFeedStockRepository : InMemoryRepository<FeedStock, Guid>, IFeedStockRepository
{
}