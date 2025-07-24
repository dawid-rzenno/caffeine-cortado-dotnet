using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class IngredientsController(
    IIngredientsRepository repository,
    INutrientsRepository nutrientsRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? term, [FromQuery] bool globalSearch = false)
    {
        IEnumerable<Ingredient> ingredients = string.IsNullOrEmpty(term) 
            ? await repository.GetAllAsync() 
            : await repository.GetAllByTermAsync(term, globalSearch);

        return Ok(ingredients);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Ingredient? ingredient = await repository.GetByIdAsync(id);

        return ingredient != null ? Ok(ingredient) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Ingredient ingredient)
    {
        ingredient = await repository.CreateAsync(ingredient);

        return Ok(ingredient);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Ingredient ingredient)
    {
        ingredient = await repository.UpdateAsync(ingredient);

        return Ok(ingredient);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
    
    [HttpPost("{id}/nutrients")]
    public async Task<IActionResult> CreateIngredientNutrient([FromRoute] int id, [FromBody] Nutrient nutrient)
    {
        await nutrientsRepository.CreateAsync(nutrient);
        
        Ingredient? ingredient = await repository.GetByIdAsync(id);

        return ingredient != null ? Ok(ingredient) : NotFound();
    }
    
    [HttpPut("{id}/nutrients")]
    public async Task<IActionResult> UpdateIngredientNutrient([FromRoute] int id, [FromBody] Nutrient nutrient)
    {
        await nutrientsRepository.UpdateAsync(nutrient);

        Ingredient? ingredient = await repository.GetByIdAsync(id);

        return ingredient != null ? Ok(ingredient) : NotFound();
    }

    [HttpDelete("{id}/nutrients/{nutrientId}")]
    public async Task<IActionResult> DeleteIngredientNutrient([FromRoute] int id, [FromRoute] int nutrientId)
    {
        await nutrientsRepository.DeleteAsync(nutrientId);
        
        Ingredient? ingredient = await repository.GetByIdAsync(id);

        return ingredient != null ? Ok(ingredient) : NotFound();
    }
}