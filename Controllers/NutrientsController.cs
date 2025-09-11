using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NutrientsController(INutrientsRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? sort,
        [FromQuery] string? sortBy,
        [FromQuery] int? size,
        [FromQuery] int? page,
        [FromQuery] string? term,
        [FromQuery] bool? globalSearch
    )
    {
        IEnumerable<Nutrient> nutrients = await repository.GetAllAsync(
            sort ?? "DESC",
            sortBy ?? "Id",
            size ?? 10,
            page ?? 1,
            term ?? "",
            globalSearch ?? false
        );

        return Ok(nutrients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Nutrient? nutrient = await repository.GetByIdAsync(id);

        return nutrient != null ? Ok(nutrient) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Nutrient nutrient)
    {
        nutrient = await repository.CreateAsync(nutrient);

        return Ok(nutrient);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Nutrient nutrient)
    {
        nutrient = await repository.UpdateAsync(nutrient);

        return Ok(nutrient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
}