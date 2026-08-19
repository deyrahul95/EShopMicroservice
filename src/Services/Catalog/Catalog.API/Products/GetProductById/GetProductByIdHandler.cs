using System.Text.Json;
using BuildingBlock.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product? Product = null);

internal class GetProductByIdQueryHandler(
    IDocumentSession session,
    ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        logger.LogInformation("Executing get product by id query. Query: {@Query}", query);

        var product = await session.LoadAsync<Product>(query.Id, ct);

        if (product is null)
        {
            logger.LogInformation("No product found with id :{@Id} in our database.", query.Id);
            throw new ProductNotFoundException(query.Id);
        }

        logger.LogInformation("Product with id: {@Id} found successfully.", product?.Id);

        logger.LogInformation(
            "Completed get product by id query. Product: {@Product}",
            JsonSerializer.Serialize(product));

        return new GetProductByIdResult(Product: product);
    }
}
