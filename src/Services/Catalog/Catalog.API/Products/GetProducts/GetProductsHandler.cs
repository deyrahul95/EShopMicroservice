using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProducts;

public record GetProductsQuery() : IQuery<GetProductsResult>;

public record GetProductsResult(IEnumerable<Product> Products);

internal class GetProductsQueryHandler(
    IDocumentSession session,
    ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken ct)
    {
        logger.LogInformation("Start executing get products query handler. Query: {@Query}", query);

        // Retrieve products from the database
        var products = await session.Query<Product>().ToListAsync(ct);
        logger.LogInformation("{@Count} products retrieved from the database.", products.Count);

        logger.LogInformation("Completed get products query handler. Count: {@Count}", products.Count);
        return new GetProductsResult(products);
    }
}
