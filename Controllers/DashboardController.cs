using cortado.DTOs;
using cortado.Models;
using cortado.Repositories;
using cortado.Services;
using Microsoft.AspNetCore.Mvc;

namespace cortado.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DashboardController(
    IGoalsRepository goalsRepository,
    IMotivationalQuotesRepository motivationalQuotesRepository
    ) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDashboardDataAsync()
    {
        IEnumerable<Goal> goals = await goalsRepository.GetAllAsync(
            "DESC",
            "Id",
            10,
            1,
            "",
            false
        );

        MotivationalQuote? motivationalQuote = await motivationalQuotesRepository.GetRandomAsync();

        return Ok(
            new DashboardDetails(motivationalQuote, null, null, goals)
        );
    }
}