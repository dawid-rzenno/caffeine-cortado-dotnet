using System.Security.Claims;

namespace cortado.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Email { get; }
    ClaimsPrincipal? User { get; }
}

public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    public ClaimsPrincipal? User => contextAccessor.HttpContext?.User;

    public string? UserId => User?.FindFirst("UserId")?.Value;

    public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;
}