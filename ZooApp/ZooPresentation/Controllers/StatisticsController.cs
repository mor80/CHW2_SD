using Microsoft.AspNetCore.Mvc;
using ZooApplication.Services;

namespace ZooPresentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StatisticsController : ControllerBase
{
    private readonly ZooStatisticsService _stats;

    public StatisticsController(ZooStatisticsService stats) => _stats = stats;

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken ct) => Ok(await _stats.GetStatsAsync(ct));
}