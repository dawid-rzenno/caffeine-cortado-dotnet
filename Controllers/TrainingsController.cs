using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class TrainingsController(
    ITrainingsRepository repository,
    IExercisesRepository exercisesRepository
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
        IEnumerable<Training> goals = await repository.GetAllAsync(
            sort ?? "DESC",
            sortBy ?? "Id",
            size ?? 10,
            page ?? 1,
            term ?? "",
            globalSearch ?? false
        );

        return Ok(goals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Training? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Training goal)
    {
        goal = await repository.CreateAsync(goal);

        return Ok(goal);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Training goal)
    {
        goal = await repository.UpdateAsync(goal);

        return Ok(goal);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);

        return success ? NoContent() : NotFound();
    }
    
    [HttpPost("{id}/exercises")]
    public async Task<IActionResult> CreateTrainingDay([FromRoute] int id, [FromBody] Exercise exercise)
    {
        await exercisesRepository.CreateAsync(exercise);
        
        Training? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }
    
    [HttpPut("{id}/exercises")]
    public async Task<IActionResult> UpdateTrainingDay([FromRoute] int id, [FromBody] Exercise exercise)
    {
        await exercisesRepository.UpdateAsync(exercise);

        Training? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }

    [HttpDelete("{id}/exercises/{exerciseId}")]
    public async Task<IActionResult> DeleteTrainingDay([FromRoute] int id, [FromRoute] int exerciseId)
    {
        await exercisesRepository.DeleteAsync(exerciseId);
        
        Training? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }
}