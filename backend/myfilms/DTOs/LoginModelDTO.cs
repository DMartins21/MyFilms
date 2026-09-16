using System.ComponentModel.DataAnnotations;

namespace myfilms.DTOs;

public class LoginModelDTO
{
    [Required(ErrorMessage = "UserName is required")]
    public string? UserName { get; set; }
    
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; set; }
}