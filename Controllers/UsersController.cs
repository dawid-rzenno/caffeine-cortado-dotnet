using cortado.DTOs;
using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(IUsersRepository repository, IUserRolesRepository userRolesRepository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        IList<UserResponse> userResponses = new List<UserResponse>();

        foreach (var user in await repository.GetAllAsync())
        {
            UserRole? userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

            if (userRole == null)
            {
                throw new Exception("Role not found");
            }
            
            userResponses.Add(new UserResponse(user, userRole));
        }
        
        return Ok(userResponses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        User? user = await repository.GetByIdAsync(id);
        
        UserRole? userRole = await userRolesRepository.GetByIdAsync(id);

        if (userRole == null)
        {
            throw new Exception("Role not found");
        }
        
        return user != null ? Ok(new UserResponse(user, userRole)) : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);
        
        return success ? NoContent() : NotFound();
    }
}