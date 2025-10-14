using cortado.Models;

namespace cortado.DTOs;

public class UpdateUserForm : Entity
{
    public string Username { get; set; }
    public int UserRoleId { get; set; }
}