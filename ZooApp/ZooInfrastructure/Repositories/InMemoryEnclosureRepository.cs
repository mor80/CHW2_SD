using ZooApplication.Abstractions;
using ZooDomain.Entities;
using ZooDomain.ValueObjects;

namespace ZooInfrastructure.Repositories;

public sealed class InMemoryEnclosureRepository : InMemoryRepository<Enclosure, EnclosureId>, IEnclosureRepository
{
}