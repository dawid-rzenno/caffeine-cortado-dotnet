using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class MassUnitsController(
    IMassUnitsRepository repository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? term, [FromQuery] bool globalSearch = false)
    {
        IEnumerable<MassUnit> massUnits = string.IsNullOrEmpty(term) 
            ? await repository.GetAllAsync() 
            : await repository.GetAllByTermAsync(term, globalSearch);

        return Ok(massUnits);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        MassUnit? massUnit = await repository.GetByIdAsync(id);

        return massUnit != null ? Ok(massUnit) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MassUnit massUnit)
    {
        massUnit = await repository.CreateAsync(massUnit);

        return Ok(massUnit);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] MassUnit massUnit)
    {
        massUnit = await repository.UpdateAsync(massUnit);

        return Ok(massUnit);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
}