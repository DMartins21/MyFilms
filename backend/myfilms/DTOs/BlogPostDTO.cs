using System.ComponentModel.DataAnnotations;
using myfilms.Models;

namespace myfilms.DTOs;

public class BlogPostDTO
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Title is required")]
    public string Title { get; set; }
    [Required(ErrorMessage = "Content is required"), MaxLength(2500, ErrorMessage = "Content is too long")]
    public string Content { get; set; }
    [Required(ErrorMessage = "Type is required")]
    public string TypeContent { get; set; }
    public DateOnly PublishedOn { get; set; }
    public DateOnly? UpdatedOn { get; set; }
    public int AuthorId { get; set; }
    public ClientDTO? Author { get; set; }
}