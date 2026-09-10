using Microsoft.AspNetCore.Identity;

namespace myfilms.Models;

public class User : IdentityUser
{
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpires { get; set; }
}