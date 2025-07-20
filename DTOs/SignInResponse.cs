using cortado.Models;

namespace cortado.DTOs;

public class SignInResponse
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public UserDetails User { get; set; }
}