using cortado.Models;

namespace cortado.DTOs;

public class UserResponse
{
    public int Id { get; set; }
    public string Username { get; set; }

    public UserResponse(User user)
    {
        Id = user.Id;
        Username = user.Username;
    }
}