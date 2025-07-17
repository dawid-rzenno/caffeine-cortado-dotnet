using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var users = await userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        User? user = await userRepository.GetOneByIdAsync(id);
        return user != null ? Ok(user) : NotFound();
    }
}