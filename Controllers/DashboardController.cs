using cortado.DTOs;
using cortado.Models;
using cortado.Repositories;
using cortado.Services;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DashboardController(
    ICurrentUserService currentUserService,
    IGoalsRepository goalsRepository,
    IMotivationalQuotesRepository motivationalQuotesRepository
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> SignIn()
    {
        IEnumerable<Goal> goals = await goalsRepository.GetAllByUserIdAsync(currentUserService.GetUserId());

        MotivationalQuote? motivationalQuote = await motivationalQuotesRepository.GetRandomAsync();

        return Ok(
            new DashboardResponse(motivationalQuote, null, null, goals)
        );
    }
}