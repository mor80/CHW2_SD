using Microsoft.AspNetCore.Mvc;
using ZooApplication.Services;
using ZooDomain.ValueObjects;

namespace ZooPresentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransfersController : ControllerBase
{
    private readonly AnimalTransferService _transfer;

    public TransfersController(AnimalTransferService transfer) => _transfer = transfer;

    [HttpPost]
    public async Task<IActionResult> Transfer([FromBody] TransferDto dto, CancellationToken ct)
    {
        await _transfer.TransferAsync(dto.AnimalId, dto.TargetEnclosureId, ct);
        return NoContent();
    }
}

public record TransferDto(AnimalId AnimalId, EnclosureId TargetEnclosureId);