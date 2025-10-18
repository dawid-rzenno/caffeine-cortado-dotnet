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
    public async Task<IActionResult> GetAll(
        [FromQuery] string? sort,
        [FromQuery] string? sortBy,
        [FromQuery] int? size,
        [FromQuery] int? page,
        [FromQuery] string? term
    )
    {
        IEnumerable<User> users = await repository.GetAllAsync(
            sort ?? "DESC",
            sortBy ?? "Id",
            size ?? 10,
            page ?? 1,
            term ?? ""
        );

        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        User? user = await repository.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound($"User with Id ${id} not found");
        }

        UserRole? userRole = await userRolesRepository.GetByIdAsync(user.UserRoleId);

        if (userRole == null)
        {
            throw new Exception($"UserRole with Id {user.UserRoleId} of User with Id ${user.Id} not found");
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
        user.UserRoleId = form.UserRoleId;

        UserRole? userRole = await userRolesRepository.GetByIdAsync(form.UserRoleId);

        if (userRole == null)
        {
            throw new Exception($"UserRole with Id {form.UserRoleId} of User with Id ${form.Id} not found");
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