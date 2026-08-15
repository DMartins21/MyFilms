using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace myfilms.Models;

public class Filme
{
    public int Id {get; set; }
    [Required]
    [MaxLength(150)]
    public string Title {get; set; }
    [MaxLength(500)]
    public string Description {get; set; }
    [Required]
    public DateOnly ReleaseDate {get; set; }
    [Required]
    [MaxLength(255)]
    public List<string> Genre {get; set; }
    [DataType(DataType.ImageUrl)]
    public string ImageUrl {get; set; }
    [DataType(DataType.ImageUrl)]
    public string ThumbnailUrl {get; set; }
}