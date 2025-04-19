using ZooApplication.Abstractions;
using ZooDomain.Entities;

namespace ZooInfrastructure.Repositories;

public sealed class InMemoryTreatmentRepository : InMemoryRepository<TreatmentCase, Guid>, ITreatmentRepository
{
}