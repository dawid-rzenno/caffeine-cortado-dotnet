using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class MilestonesController(IMilestonesRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Milestone> milestones = await repository.GetAllAsync();
        
        return Ok(milestones);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        Milestone? milestone = await repository.GetByIdAsync(id);
        
        return milestone != null ? Ok(milestone) : NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Milestone milestone)
    {
        milestone = await repository.CreateAsync(milestone);
        
        return Ok(milestone);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] Milestone milestone)
    {
        milestone = await repository.UpdateAsync(milestone);

        return Ok(milestone);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);
        
        return success ? NoContent() : NotFound();
    }
}