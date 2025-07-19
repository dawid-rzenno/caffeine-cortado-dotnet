using cortado.Models;

namespace cortado.DTOs;

public class UserResponse
{
    public int Id { get; set; }
    public string Username { get; set; }
    public DateTime Timestamp { get; set; }
    public int UserId { get; set; }
    public UserRole UserRole { get; set; }

    public UserResponse(User user, UserRole role)
    {
        Id = user.Id;
        Username = user.Username;
        Timestamp = user.Timestamp;
        UserId = user.UserId;
        UserRole = role;
    }
}