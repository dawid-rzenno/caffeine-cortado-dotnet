using cortado.DTOs;
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
    IDietMealsRepository dietMealsRepository
) : ControllerBase
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
        IEnumerable<Diet> diets = await repository.GetAllAsync(
            sort ?? "DESC",
            sortBy ?? "Id",
            size ?? 10,
            page ?? 1,
            term ?? "",
            globalSearch ?? false
        );

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
    public async Task<IActionResult> CreateDietMeal([FromRoute] int id, [FromBody] DietMealForm form)
    {
        await dietMealsRepository.CreateAsync(
            new DietMeal
            {
                DietId = id,
                MealId = form.MealId,
                MealIndex = form.MealIndex,
                MealDayIndex = form.MealDayIndex
            }
        );

        DietDetails? dietDetails = await repository.GetByIdAsync(id);

        return dietDetails != null ? Ok(dietDetails) : NotFound();
    }

    [HttpDelete("{id}/meals/{dietMealId}")]
    public async Task<IActionResult> DeleteDietMeal([FromRoute] int id, [FromRoute] int dietMealId)
    {
        await dietMealsRepository.DeleteAsync(dietMealId);

        DietDetails? dietDetails = await repository.GetByIdAsync(id);

        return dietDetails != null ? Ok(dietDetails) : NotFound();
    }
}