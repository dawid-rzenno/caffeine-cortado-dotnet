using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NutrientTypesController(
    INutrientTypesRepository repository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllTypes([FromQuery] string? term, [FromQuery] bool globalSearch = false)
    {
        IEnumerable<NutrientType> nutrients = string.IsNullOrEmpty(term)
            ? await repository.GetAllAsync() 
            : await repository.GetAllByTermAsync(term, globalSearch);

        return Ok(nutrients);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTypeById([FromRoute] int id)
    {
        NutrientType? nutrientType = await repository.GetByIdAsync(id);

        return nutrientType != null ? Ok(nutrientType) : NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateType([FromBody] NutrientType nutrientType)
    {
        nutrientType = await repository.CreateAsync(nutrientType);

        return Ok(nutrientType);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteType([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
}