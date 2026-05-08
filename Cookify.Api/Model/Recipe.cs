namespace Cookify.Api.Model;

public class Recipe
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string AuthorId { get; set; } = null!;
    public User Author { get; set; } = null!;
}