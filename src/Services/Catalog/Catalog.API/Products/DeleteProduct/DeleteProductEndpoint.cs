using Carter;
using Catalog.API.Constants;
using MediatR;

namespace Catalog.API.Products.DeleteProduct;

public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(ProductConstants.PRODUCT_ROUTE + "/{id}", async (
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
        .WithTags(ProductConstants.PRODUCT_TAG)
        .WithName(ProductConstants.DELETE_PRODUCT_NAME)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary(ProductConstants.DELETE_PRODUCT_DESCRIPTION)
        .WithDescription(ProductConstants.DELETE_PRODUCT_DESCRIPTION);
    }
}
