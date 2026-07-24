namespace Catalog.API.Models;

public record ProductDto(
    Guid Id,
    string Name,
    List<string> Categories,
    string Description,
    string ImageUrl,
    decimal Price,
    DateTime UpdatedAt
);
