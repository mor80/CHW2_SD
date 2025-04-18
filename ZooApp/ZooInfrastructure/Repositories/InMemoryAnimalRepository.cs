using ZooApplication.Abstractions;
using ZooDomain.Entities;
using ZooDomain.ValueObjects;

namespace ZooInfrastructure.Repositories;

public sealed class InMemoryAnimalRepository : InMemoryRepository<Animal, AnimalId>, IAnimalRepository
{
}