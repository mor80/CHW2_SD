using ZooDomain.Entities;
using ZooDomain.ValueObjects;

namespace ZooApplication.Abstractions;

public interface IAnimalRepository : IRepository<Animal, AnimalId>
{
}