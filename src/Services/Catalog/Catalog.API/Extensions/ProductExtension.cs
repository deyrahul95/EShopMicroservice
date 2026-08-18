using Catalog.API.Models;

namespace Catalog.API.Extensions;

public static class ProductExtension
{
    public static ProductDto ToDto(this Product product) => new(
        Id: product.Id,
        Name: product.Name,
        Categories: product.Category,
        Description: product.Description,
        ImageUrl: product.ImageUrl,
        Price: product.Price,
        UpdatedAt: product.UpdatedAt);

    public static IReadOnlyList<ProductDto> ToDtoList(this IEnumerable<Product> products)
    {
        if (products.Any() == false)
        {
            return [];
        }

        return [.. products.Select(p => p.ToDto())];
    }
}