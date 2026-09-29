namespace myfilms.DTOs;

public class ClientDTO
{
    public string Name { get; set; }
    public string? LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public DateOnly BirthDate { get; set; }
}