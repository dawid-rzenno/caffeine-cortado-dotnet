using cortado.Models;
using cortado.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
public class UserRolesController(IUserRolesRepository repository) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? sort,
        [FromQuery] string? sortBy,
        [FromQuery] int? size,
        [FromQuery] int? page,
        [FromQuery] string? term,
        [FromQuery] bool? globalSearch
    )
    {
        IEnumerable<UserRole> userRoles = await repository.GetAllAsync(
            sort ?? "DESC",
            sortBy ?? "Id",
            size ?? 10,
            page ?? 1,
            term ?? "",
            globalSearch ?? false
        );
        
        return Ok(userRoles);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        UserRole? userRole = await repository.GetByIdAsync(id);
        
        return userRole != null ? Ok(userRole) : NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserRole userRole)
    {
        userRole = await repository.CreateAsync(userRole);
        
        return Ok(userRole);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UserRole userRole)
    {
        userRole = await repository.UpdateAsync(userRole);

        return Ok(userRole);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var success = await repository.DeleteAsync(id);
        
        return success ? NoContent() : NotFound();
    }
}