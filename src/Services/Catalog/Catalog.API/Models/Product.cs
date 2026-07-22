namespace Catalog.API.Models;

public class Product
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public List<string> Category { get; set; } = [];
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
