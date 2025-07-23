using cortado.Models;

namespace cortado.DTOs;

public class UserDetails : Entity
{
    public string Username { get; set; }
    public UserRole Role { get; set; }

    public UserDetails(User user, UserRole role)
    {
        Id = user.Id;
        Username = user.Username;
        Timestamp = user.Timestamp;
        UserId = user.UserId;
        Role = role;
    }
}