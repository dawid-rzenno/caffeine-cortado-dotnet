using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class NutrientsController(
    INutrientTypesRepository typesRepository,
    INutrientNamesRepository namesRepository
) : ControllerBase
{
    [HttpGet("types")]
    public async Task<IActionResult> GetAllTypes([FromQuery] string? term, [FromQuery] bool globalSearch = false)
    {
        IEnumerable<NutrientType> nutrients = string.IsNullOrEmpty(term)
            ? await typesRepository.GetAllAsync() 
            : await typesRepository.GetAllByTermAsync(term, globalSearch);

        return Ok(nutrients);
    }
    
    [HttpGet("types/{id}")]
    public async Task<IActionResult> GetTypeById([FromRoute] int id)
    {
        NutrientType? nutrientType = await typesRepository.GetByIdAsync(id);

        return nutrientType != null ? Ok(nutrientType) : NotFound();
    }
    
    [HttpPost("types")]
    public async Task<IActionResult> CreateType([FromBody] NutrientType nutrientType)
    {
        nutrientType = await typesRepository.CreateAsync(nutrientType);

        return Ok(nutrientType);
    }

    [HttpDelete("types/{id}")]
    public async Task<IActionResult> DeleteType([FromRoute] int id)
    {
        var success = await typesRepository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
    
    [HttpGet("names")]
    public async Task<IActionResult> GetAllNames([FromQuery] string? term, [FromQuery] bool globalSearch = false)
    {
        IEnumerable<NutrientName> nutrients = string.IsNullOrEmpty(term) 
            ? await namesRepository.GetAllAsync() 
            : await namesRepository.GetAllByTermAsync(term, globalSearch);

        return Ok(nutrients);
    }
    
    [HttpGet("names/{id}")]
    public async Task<IActionResult> GetNameById([FromRoute] int id)
    {
        NutrientName? nutrientName = await namesRepository.GetByIdAsync(id);

        return nutrientName != null ? Ok(nutrientName) : NotFound();
    }
    
    [HttpPost("names")]
    public async Task<IActionResult> CreateName([FromBody] NutrientName nutrientName)
    {
        nutrientName = await namesRepository.CreateAsync(nutrientName);

        return Ok(nutrientName);
    }

    [HttpDelete("names/{id}")]
    public async Task<IActionResult> DeleteName([FromRoute] int id)
    {
        var success = await namesRepository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
}