using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class GoalsController(IGoalsRepository repository) : ControllerBase
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
}