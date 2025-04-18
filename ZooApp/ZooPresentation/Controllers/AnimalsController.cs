using Microsoft.AspNetCore.Mvc;
using ZooApplication.Abstractions;
using ZooDomain.Entities;
using ZooDomain.Enums;
using ZooDomain.ValueObjects;

namespace ZooPresentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalsController : ControllerBase
{
    private readonly IAnimalRepository _animals;

    public AnimalsController(IAnimalRepository animals) => _animals = animals;

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct) =>
        Ok(await _animals.ListAsync(ct));

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] CreateAnimalDto dto, CancellationToken ct)
    {
        var animal = new Animal(dto.Species, dto.Name, dto.BirthDate, dto.Gender,
            dto.FavoriteFood, dto.EnclosureId);
        await _animals.AddAsync(animal, ct);
        await _animals.SaveChangesAsync(ct);
        return CreatedAtAction(nameof(GetById), new { id = animal.Id }, animal);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var animal = await _animals.GetAsync(new AnimalId(id), ct);
        return animal is null ? NotFound() : Ok(animal);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var animal = await _animals.GetAsync(new AnimalId(id), ct);
        if (animal is null) return NotFound();
        await _animals.RemoveAsync(animal, ct);
        await _animals.SaveChangesAsync(ct);
        return NoContent();
    }
}

public record CreateAnimalDto(
    string Species,
    string Name,
    DateTime BirthDate,
    Gender Gender,
    DietType FavoriteFood,
    EnclosureId EnclosureId);