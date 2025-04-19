using ZooApplication.Abstractions;
using ZooDomain.Entities;

namespace ZooInfrastructure.Repositories;

public sealed class InMemoryFeedingScheduleRepository : InMemoryRepository<FeedingSchedule, Guid>,
    IFeedingScheduleRepository
{
}