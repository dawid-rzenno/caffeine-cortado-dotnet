using cortado.DTOs;
using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(UserRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<User> users = await repository.GetAllAsync();
        
        return Ok(users.Select(u => new UserResponse(u)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        User? user = await repository.GetByIdAsync(id);
        
        return user != null ? Ok(new UserResponse(user)) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);
        
        return success ? NoContent() : NotFound();
    }
}