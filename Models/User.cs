namespace cortado.Models;

public class User : Entity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public int RoleId { get; set; }
}