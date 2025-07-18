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
    UserRepository userRepository,
    PasswordService passwordService
) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        User? user = await userRepository.GetByUsernameAsync(request.Username);

        if (user == null || !passwordService.VerifyPassword(request.Password, user.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        var token = jwtTokenService.GenerateToken(request.Username);
        return Ok(token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var userId = await userRepository.CreateAsync(
            new User
            {
                Username = request.Username,
                Password = passwordService.HashPassword(request.Password)
            }
        );

        var user = await userRepository.GetByIdAsync(userId);

        return Created($"api/v1/users/{userId}", user);
    }
}