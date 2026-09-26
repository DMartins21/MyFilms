using System.ComponentModel.DataAnnotations;

namespace myfilms.Models;

public class Client
{
    public int Id  { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    public string LastName { get; set; }
    [Url(ErrorMessage = "ProfileUrl is not valid")]
    public string ProfilePictureUrl { get; set; }
    [Required(ErrorMessage = "BirthDate is required")]
    public DateOnly BirthDate { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
    public ICollection<Filme> FavoriteFilmes { get; set; }
    public ICollection<BlogPost> Posts { get; set; }
}