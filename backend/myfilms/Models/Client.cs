using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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
    
    [JsonIgnore]
    public string UserId { get; set; }
    [JsonIgnore]
    public virtual User User { get; set; }
    public virtual ICollection<Filme> FavoriteFilmes { get; set; }
    public virtual ICollection<BlogPost> Posts { get; set; }
}