using Microsoft.AspNetCore.Mvc;
using ZooApplication.Services;
using ZooDomain.ValueObjects;

namespace ZooPresentation.Controllers;

[ApiController]
[Route("api/health")]
public class HealthController : ControllerBase
{
    private readonly HealthService _health;
    public HealthController(HealthService health) => _health = health;

    [HttpPost("start")]
    public async Task<IActionResult> Start(StartDto dto, CancellationToken ct) =>
        Ok(await _health.StartTreatmentAsync(dto.AnimalId, dto.Diagnosis, ct));

    [HttpPost("finish/{caseId:guid}")]
    public async Task<IActionResult> Finish(Guid caseId, CancellationToken ct)
    {
        await _health.FinishTreatmentAsync(caseId, ct);
        return NoContent();
    }
}

public record StartDto(AnimalId AnimalId, string Diagnosis);