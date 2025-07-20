using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class DietsController(
    IDietsRepository repository,
    IMealsRepository mealsRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Diet> diets = await repository.GetAllAsync();

        return Ok(diets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Diet? diet = await repository.GetByIdAsync(id);

        return diet != null ? Ok(diet) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Diet diet)
    {
        diet = await repository.CreateAsync(diet);

        return Ok(diet);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Diet diet)
    {
        diet = await repository.UpdateAsync(diet);

        return Ok(diet);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
    
    [HttpPost("{id}/meals")]
    public async Task<IActionResult> CreateDietMeal([FromRoute] int id, [FromBody] Meal meal)
    {
        await mealsRepository.CreateAsync(meal);
        
        Diet? diet = await repository.GetByIdAsync(id);

        return diet != null ? Ok(diet) : NotFound();
    }
    
    [HttpPut("{id}/meals")]
    public async Task<IActionResult> UpdateDietMeal([FromRoute] int id, [FromBody] Meal meal)
    {
        await mealsRepository.UpdateAsync(meal);

        Diet? diet = await repository.GetByIdAsync(id);

        return diet != null ? Ok(diet) : NotFound();
    }

    [HttpDelete("{id}/meals/{mealId}")]
    public async Task<IActionResult> DeleteDietMeal([FromRoute] int id, [FromRoute] int mealId)
    {
        await mealsRepository.DeleteAsync(mealId);
        
        Diet? diet = await repository.GetByIdAsync(id);

        return diet != null ? Ok(diet) : NotFound();
    }
}