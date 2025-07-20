using cortado.DTOs;
using cortado.Models;
using cortado.Repositories;
using cortado.Services;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    JwtTokenService jwtTokenService,
    IUsersRepository usersRepository,
    IUserRolesRepository userRolesRepository,
    PasswordService passwordService
) : ControllerBase
{
    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] SignInForm form)
    {
        User? user = await usersRepository.GetByUsernameAsync(form.Username);

        if (user == null || !passwordService.VerifyPassword(form.Password, user.Password))
        {
            return Unauthorized("Invalid credentials.");
        }

        return Ok(jwtTokenService.GenerateToken(user));
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
        
        var user = await usersRepository.CreateAsync(
            new User
            {
                Username = form.Username,
                Password = passwordService.HashPassword(form.Password),
                RoleId = defaultUserRoleId
            }
        );

        return Created("", new UserDetails(user, userRole));
    }
}