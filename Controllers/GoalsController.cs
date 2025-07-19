using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class GoalsController(
    IGoalsRepository repository,
    IMilestonesRepository milestonesRepository
) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Goal> goals = await repository.GetAllAsync();

        return Ok(goals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Goal? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Goal goal)
    {
        goal = await repository.CreateAsync(goal);

        return Ok(goal);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Goal goal)
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
    
    [HttpPost("{id}/milestones")]
    public async Task<IActionResult> CreateGoalMilestone([FromRoute] int id, [FromBody] Milestone milestone)
    {
        await milestonesRepository.CreateAsync(milestone);
        
        Goal? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }
    
    [HttpPut("{id}/milestones")]
    public async Task<IActionResult> UpdateGoalMilestone([FromRoute] int id, [FromBody] Milestone milestone)
    {
        await milestonesRepository.UpdateAsync(milestone);

        Goal? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }

    [HttpDelete("{id}/milestones/{milestoneId}")]
    public async Task<IActionResult> DeleteGoalMilestone([FromRoute] int id, [FromRoute] int milestoneId)
    {
        await milestonesRepository.DeleteAsync(milestoneId);
        
        Goal? goal = await repository.GetByIdAsync(id);

        return goal != null ? Ok(goal) : NotFound();
    }
}