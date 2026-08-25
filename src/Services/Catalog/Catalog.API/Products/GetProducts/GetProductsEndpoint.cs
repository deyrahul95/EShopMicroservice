using Carter;
using Catalog.API.Constants;
using Catalog.API.Extensions;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.GetProducts;

public record GetProductsRequest(
    int? Page = CatalogConstant.DefaultPageNumber,
    int? PageSize = CatalogConstant.DefaultPageSize);

public record GetProductsResponse(
    long Page,
    long PageSize,
    long Total,
    long TotalPages,
    IReadOnlyList<ProductDto> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ProductRouteConstant.PRODUCT_ROUTE,
            async (
                [AsParameters] GetProductsRequest request,
                ISender sender,
                CancellationToken ct) =>
        {
            var query = ToQuery(request);
            var result = await sender.Send(query, ct);
            var response = ToResponse(result);
            return Results.Ok(response);
        })
        .WithTags(ProductRouteConstant.PRODUCT_TAG)
        .WithName(ProductRouteConstant.GET_PRODUCTS_NAME)
        .Produces<GetProductsResponse>(StatusCodes.Status200OK)
        .WithSummary(ProductRouteConstant.GET_PRODUCTS_DESCRIPTION)
        .WithDescription(ProductRouteConstant.GET_PRODUCTS_DESCRIPTION);
    }

    private static GetProductsQuery ToQuery(GetProductsRequest request)
        => new(
            PageNumber: request.Page ?? CatalogConstant.DefaultPageNumber,
            PageSize: request.PageSize ?? CatalogConstant.DefaultPageSize);

    private static GetProductsResponse ToResponse(GetProductsResult result)
        => new(
            Page: result.Products.PageNumber,
            PageSize: result.Products.PageSize,
            Total: result.Products.TotalItemCount,
            TotalPages: result.Products.PageCount,
            Products: result.Products.ToDtoList());
}
