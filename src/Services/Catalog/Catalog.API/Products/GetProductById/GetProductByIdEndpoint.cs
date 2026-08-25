using Carter;
using Catalog.API.Constants;
using Catalog.API.Extensions;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.GetProductById;

public record GetProductByIdResponse(ProductDto Product);

public class GetProductByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(ProductRouteConstant.PRODUCT_ROUTE + "/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken ct) =>
        {
            var query = new GetProductByIdQuery(id);
            var result = await sender.Send(query, ct);

            if (result is null || result?.Product is null)
            {
                return Results.NotFound($"No product found with id `{id}`.");
            }

            var response = ToResponse(result);
            return Results.Ok(response);
        })
        .WithTags(ProductRouteConstant.PRODUCT_TAG)
        .WithName(ProductRouteConstant.GET_PRODUCT_BY_ID_NAME)
        .Produces<GetProductByIdResponse>(
            StatusCodes.Status200OK,
            ProductRouteConstant.JSON_CONTENT_TYPE)
        .ProducesProblem(
            StatusCodes.Status404NotFound,
            ProductRouteConstant.JSON_CONTENT_TYPE)
        .WithDescription(ProductRouteConstant.GET_PRODUCT_BY_ID_DESCRIPTION)
        .WithSummary(ProductRouteConstant.GET_PRODUCT_BY_ID_DESCRIPTION);
    }

    private static GetProductByIdResponse ToResponse(GetProductByIdResult result)
        => new(Product: result.Product!.ToDto());
}
