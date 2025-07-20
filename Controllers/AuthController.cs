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
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginForm form)
    {
        User? user = await usersRepository.GetByUsernameAsync(form.Username);

        if (user == null || !passwordService.VerifyPassword(form.Password, user.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(jwtTokenService.GenerateToken(user));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterForm form)
    {
        var user = await usersRepository.CreateAsync(
            new User
            {
                Username = form.Username,
                Password = passwordService.HashPassword(form.Password),
                RoleId = 1
            }
        );
        
        var userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
        {
            throw new Exception("Role not found.");
        }

        return Created("", new UserDetails(user, userRole));
    }
}