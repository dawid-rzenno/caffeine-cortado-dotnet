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
    ITrainingDaysRepository trainingDaysRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Training> goals = await repository.GetAllAsync();

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
    
    [HttpPost("{id}/training-days")]
    public async Task<IActionResult> CreateTrainingDay([FromRoute] int id, [FromBody] TrainingDay trainingDay)
    {
        await trainingDaysRepository.CreateAsync(trainingDay);
        
        Training? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }
    
    [HttpPut("{id}/training-days")]
    public async Task<IActionResult> UpdateTrainingDay([FromRoute] int id, [FromBody] TrainingDay trainingDay)
    {
        await trainingDaysRepository.UpdateAsync(trainingDay);

        Training? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }

    [HttpDelete("{id}/training-days/{trainingDayId}")]
    public async Task<IActionResult> DeleteTrainingDay([FromRoute] int id, [FromRoute] int trainingDayId)
    {
        await trainingDaysRepository.DeleteAsync(trainingDayId);
        
        Training? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }
}