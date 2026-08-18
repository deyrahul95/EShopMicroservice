using Carter;
using Catalog.API.Constants;
using Catalog.API.Extensions;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.GetProducts;

public record GetProductsResponse(IReadOnlyList<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ProductConstants.PRODUCT_ROUTE,
            async (ISender sender, CancellationToken ct) =>
        {
            var query = new GetProductsQuery();
            var result = await sender.Send(query, ct);
            var response = ToResponse(result);
            return Results.Ok(response);
        })
        .WithTags(ProductConstants.PRODUCT_TAG)
        .WithName(ProductConstants.GET_PRODUCTS_NAME)
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .WithSummary(ProductConstants.GET_PRODUCTS_DESCRIPTION)
        .WithDescription(ProductConstants.GET_PRODUCTS_DESCRIPTION);
    }

    private static GetProductsResponse ToResponse(GetProductsResult result)
        => new(Products: result.Products.ToDtoList());
}
