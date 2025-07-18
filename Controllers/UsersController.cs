using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController(UserRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await repository.GetAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await repository.GetByIdAsync(id);
        return user is null ? NotFound() : Ok(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create(User user)
    {
        var id = await repository.CreateAsync(user);
        return CreatedAtAction(nameof(GetById), new { id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, User user)
    {
        if (id != user.Id) return BadRequest();
        var success = await repository.UpdateAsync(user);
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await repository.DeleteAsync(id);
        return success ? NoContent() : NotFound();
    }
}