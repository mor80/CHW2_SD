using Microsoft.AspNetCore.Mvc;
using ZooApplication.Abstractions;
using ZooDomain.Entities;
using ZooDomain.Enums;

namespace ZooPresentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnclosuresController : ControllerBase
{
    private readonly IEnclosureRepository _enclosures;

    public EnclosuresController(IEnclosureRepository enclosures) => _enclosures = enclosures;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _enclosures.ListAsync(ct));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateEnclosureDto dto, CancellationToken ct)
    {
        var enclosure = new Enclosure(dto.Type, dto.MaxCapacity);
        await _enclosures.AddAsync(enclosure, ct);
        await _enclosures.SaveChangesAsync(ct);
        return Created(enclosure.Id.ToString(), enclosure);
    }
}

public record CreateEnclosureDto(EnclosureType Type, int MaxCapacity);