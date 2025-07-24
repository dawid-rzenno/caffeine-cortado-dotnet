using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class MealsController(
    IMealsRepository repository,
    IIngredientsRepository ingredientsRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? term, [FromQuery] bool globalSearch = false)
    {
        IEnumerable<Meal> meals = string.IsNullOrEmpty(term) 
            ? await repository.GetAllAsync() 
            : await repository.GetAllByTermAsync(term, globalSearch);

        return Ok(meals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Meal? meal = await repository.GetByIdAsync(id);

        return meal != null ? Ok(meal) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Meal meal)
    {
        meal = await repository.CreateAsync(meal);

        return Ok(meal);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Meal meal)
    {
        meal = await repository.UpdateAsync(meal);

        return Ok(meal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
    
    [HttpPost("{id}/ingredients")]
    public async Task<IActionResult> CreateMealIngredient([FromRoute] int id, [FromBody] Ingredient ingredient)
    {
        await ingredientsRepository.CreateAsync(ingredient);
        
        Meal? meal = await repository.GetByIdAsync(id);

        return meal != null ? Ok(meal) : NotFound();
    }
    
    [HttpPut("{id}/ingredients")]
    public async Task<IActionResult> UpdateMealIngredient([FromRoute] int id, [FromBody] Ingredient ingredient)
    {
        await ingredientsRepository.UpdateAsync(ingredient);

        Meal? meal = await repository.GetByIdAsync(id);

        return meal != null ? Ok(meal) : NotFound();
    }

    [HttpDelete("{id}/ingredients/{ingredientId}")]
    public async Task<IActionResult> DeleteMealIngredient([FromRoute] int id, [FromRoute] int ingredientId)
    {
        await ingredientsRepository.DeleteAsync(ingredientId);
        
        Meal? meal = await repository.GetByIdAsync(id);

        return meal != null ? Ok(meal) : NotFound();
    }
}