using cortado.Models;

namespace cortado.DTOs;

public class UpdateUserForm : Entity
{
    public string Username { get; set; }
    public int RoleId { get; set; }
}