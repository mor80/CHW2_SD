using Microsoft.AspNetCore.Mvc;
using ZooApplication.Abstractions;
using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.ValueObjects;

namespace ZooPresentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeedingSchedulesController : ControllerBase
{
    private readonly IFeedingScheduleRepository _schedules;

    public FeedingSchedulesController(IFeedingScheduleRepository schedules) => _schedules = schedules;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _schedules.ListAsync(ct));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateScheduleDto dto, CancellationToken ct)
    {
        var schedule = new FeedingSchedule(dto.AnimalId, dto.Time, dto.Food);
        await _schedules.AddAsync(schedule, ct);
        await _schedules.SaveChangesAsync(ct);
        return Created(schedule.Id.ToString(), schedule);
    }
}

public record CreateScheduleDto(AnimalId AnimalId, TimeOnly Time, DietType Food);