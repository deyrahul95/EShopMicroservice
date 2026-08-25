using Carter;
using Catalog.API.Constants;
using Catalog.API.Extensions;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.GetProductByCategory;

public record GetProductByCategoryResponse(IReadOnlyList<ProductDto> Products);

public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ProductRouteConstant.GET_PRODUCT_BY_CATEGORY_ROUTE + "/{category}", async (
            string category,
            ISender sender,
            CancellationToken ct) =>
        {
            var query = new GetProductByCategoryQuery(Category: category);
            var result = await sender.Send(query, ct);
            var response = ToResponse(result);
            return Results.Ok(response);
        })
        .WithTags(ProductRouteConstant.PRODUCT_TAG)
        .WithName(ProductRouteConstant.GET_PRODUCT_BY_CATEGORY_NAME)
        .Produces<GetProductByCategoryResponse>(
            StatusCodes.Status200OK,
            ProductRouteConstant.JSON_CONTENT_TYPE)
        .WithDescription(ProductRouteConstant.GET_PRODUCT_BY_CATEGORY_DESCRIPTION)
        .WithSummary(ProductRouteConstant.GET_PRODUCT_BY_CATEGORY_DESCRIPTION);
    }

    private static GetProductByCategoryResponse ToResponse(GetProductByCategoryResult result)
    {
        var productDtoList = result.Products
            .Select(p => p.ToDto())
            .ToList();
        return new GetProductByCategoryResponse(productDtoList);
    }
}
