﻿using cortado.DTOs;
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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        User? user = await usersRepository.GetByUsernameAsync(request.Username);

        if (user == null || !passwordService.VerifyPassword(request.Password, user.Password))
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(jwtTokenService.GenerateToken(user));
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var user = await usersRepository.CreateAsync(
            new User
            {
                Username = request.Username,
                Password = passwordService.HashPassword(request.Password),
                RoleId = 1
            }
        );
        
        var userRole = await userRolesRepository.GetByIdAsync(user.RoleId);

        if (userRole == null)
        {
            throw new Exception("Role not found.");
        }

        return Created("", new UserResponse(user, userRole));
    }
}