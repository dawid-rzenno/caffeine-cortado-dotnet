using System.Security.Claims;

namespace cortado.Services;

public interface ICurrentUserService
{
    ClaimsPrincipal? User { get; }
    
    int? GetUserId();
}

public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    public ClaimsPrincipal? User => contextAccessor.HttpContext?.User;

    public int? GetUserId()
    {
        string? userId = User?.FindFirst("UserId")?.Value;

        return !string.IsNullOrEmpty(userId) ? int.Parse(userId) : null;
    }
}