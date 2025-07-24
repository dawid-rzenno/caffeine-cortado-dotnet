using cortado.DTOs;
using cortado.Models;
using cortado.Repositories;
using cortado.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace cortado.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    JwtTokenService jwtTokenService,
    IUsersRepository usersRepository,
    IUserRolesRepository userRolesRepository,
    PasswordService passwordService,
    ICurrentUserService currentUserService
) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInForm form)
    {
        User? user = await usersRepository.GetByUsernameAsync(form.Username);

        if (user == null)
        {
            return NotFound($"User with Username ${form.Username} not found");
        }

        if (!passwordService.VerifyPassword(form.Password, user.Password))
        {
            return Unauthorized("Invalid credentials.");
        }
        
        UserRole? userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
        {
            throw new Exception($"UserRole with Id {user.RoleId} of User with Id ${user.Id} not found");
        }

        return Ok(jwtTokenService.GenerateToken(user, userRole));
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpForm form)
    {
        var defaultUserRoleId = 1;
        
        var userRole = await userRolesRepository.GetByIdAsync(defaultUserRoleId);

        if (userRole == null)
        {
            throw new Exception($"Role with Id {defaultUserRoleId} not found.");
        }

        try
        {
            User user = await usersRepository.CreateAsync(
                new User
                {
                    Username = form.Username,
                    Password = form.Password,
                    RoleId = defaultUserRoleId
                }
            );

            return Ok(new { Id = user.Id });
        }
        catch (SqlException ex) when (ex.Number is 2627 or 2601)
        {
            return Conflict($"User with Username {form.Username} already exists.");
        }
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Get()
    {
        User? user = await usersRepository.GetByIdAsync(currentUserService.GetUserId());
        
        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }
        
        UserRole? userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
        {
            throw new Exception($"UserRole with Id {user.RoleId} of User with Id ${user.Id} not found");
        }
        
        return Ok(new UserDetails(user, userRole));
    }
    
    [HttpPut("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm form)
    {
        User? user = await usersRepository.GetByIdAsync(form.Id);
        
        if (user == null)
        {
            return NotFound($"User with Id ${form.Id} not found");
        }

        user.Password = form.Password;
        user = await usersRepository.UpdatePasswordAsync(user);
        
        UserRole? userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
        {
            throw new Exception($"UserRole with Id {user.RoleId} of User with Id ${user.Id} not found");
        }

        return Ok(new UserDetails(user, userRole));
    }
}