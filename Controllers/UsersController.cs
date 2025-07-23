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
    public async Task<IActionResult> GetAll([FromQuery] string? term)
    {
        IList<UserDetails> userResponses = new List<UserDetails>();
        
        IEnumerable<User> users = string.IsNullOrEmpty(term) 
            ? await repository.GetAllAsync() 
            : await repository.GetAllByTermAsync(term);

        foreach (var user in users)
        {
            UserRole? userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

            if (userRole == null)
            {
                throw new Exception($"UserRole with Id {user.RoleId} of User with Id ${user.Id} not found");
            }
            
            userResponses.Add(new UserDetails(user, userRole));
        }
        
        return Ok(userResponses);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        User? user = await repository.GetByIdAsync(id);
        
        if (user == null)
        {
            return NotFound($"User with Id ${id} not found");
        }
        
        UserRole? userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
        {
            throw new Exception($"UserRole with Id {user.RoleId} of User with Id ${user.Id} not found");
        }

        return Ok(new UserDetails(user, userRole));
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserForm form)
    {
        User? user = await repository.GetByIdAsync(form.Id);
        
        if (user == null)
        {
            return NotFound($"User with Id ${form.Id} not found");
        }
        
        user.Username = form.Username;
        user.RoleId = form.RoleId;
        
        UserRole? userRole = await userRolesRepository.GetByIdAsync(form.RoleId);

        if (userRole == null)
        {
            throw new Exception($"UserRole with Id {form.RoleId} of User with Id ${form.Id} not found");
        }
        
        user = await repository.UpdateAsync(user);

        return Ok(new UserDetails(user, userRole));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);
        
        return success ? NoContent() : NotFound();
    }
}