using ZooApplication.Abstractions;

namespace ZooApplication.Services;

public record ZooStats(int AnimalCount, int EnclosuresTotal, int EnclosuresFree);

public class ZooStatisticsService
{
    private readonly IAnimalRepository _animals;
    private readonly IEnclosureRepository _enclosures;

    public ZooStatisticsService(IAnimalRepository animals, IEnclosureRepository enclosures)
    {
        _animals = animals;
        _enclosures = enclosures;
    }

    public async Task<ZooStats> GetStatsAsync(CancellationToken ct = default)
    {
        var animals = await _animals.ListAsync(ct);
        var enclosures = await _enclosures.ListAsync(ct);

        int free = enclosures.Count(e => e.AnimalIds.Count < e.MaxCapacity);

        return new ZooStats(animals.Count(), enclosures.Count(), free);
    }
}