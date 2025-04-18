using ZooDomain.Entities;

namespace ZooApplication.Abstractions;

public interface ITreatmentRepository : IRepository<TreatmentCase, Guid>
{
}