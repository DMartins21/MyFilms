using System.ComponentModel.DataAnnotations;

namespace myfilms.DTOs;

public class FilmeDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    [MaxLength(500, ErrorMessage = "Description is too long")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Release Date is required")]
    public DateOnly ReleaseDate { get; set; }
    [Required(ErrorMessage = "Genre is required")]
    [MaxLength(255, ErrorMessage = "Genre is too long")]
    public List<string> Genre { get; set; }
    [DataType(DataType.ImageUrl)]
    public string ImageUrl { get; set; }
    [Required(ErrorMessage = "Thumbnail is required"),DataType(DataType.ImageUrl)]
    public string ThumbnailUrl { get; set; }
}