using Carter;
using Catalog.API.Constants;
using MediatR;

namespace Catalog.API.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(ProductRouteConstant.PRODUCT_ROUTE + "/{id}", async (
            Guid id,
            ISender sender,
            CancellationToken ct) =>
        {
            var command = new DeleteProductCommand(Id: id);
            var result = await sender.Send(command, ct);

            if (result.Completed)
            {
                return Results.NoContent();
            }

            return Results.NotFound();
        })
        .WithTags(ProductRouteConstant.PRODUCT_TAG)
        .WithName(ProductRouteConstant.DELETE_PRODUCT_NAME)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary(ProductRouteConstant.DELETE_PRODUCT_DESCRIPTION)
        .WithDescription(ProductRouteConstant.DELETE_PRODUCT_DESCRIPTION);
    }
}
