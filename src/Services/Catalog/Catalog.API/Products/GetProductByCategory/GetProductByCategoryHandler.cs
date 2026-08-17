using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryQueryHandler(
    IDocumentSession session,
    ILogger<GetProductByCategoryQueryHandler> logger)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken ct)
    {
        logger.LogInformation("Executing get product by category query. Query: {@Query}", query);

        var products = await session.Query<Product>()
            .Where(p => p.Category
                .Any(c => c.Contains(
                    query.Category,
                    StringComparison.OrdinalIgnoreCase)))
            .ToListAsync(ct);

        logger.LogInformation(
            "{@Count} products found for category: {@Category} in the database.",
            products.Count,
            query.Category);

        logger.LogInformation("Completed get products by category query handler.");
        return new GetProductByCategoryResult(Products: products);
    }
}
