namespace cortado.Services;

public interface ICurrentUserService
{
    int GetUserId();
}

public class CurrentUserService(IHttpContextAccessor contextAccessor) : ICurrentUserService
{
    public int GetUserId()
    {
        string? userId = contextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new Exception("User not found.");
        }

        return int.Parse(userId);
    }
}