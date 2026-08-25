using Carter;
using Catalog.API.Constants;
using MediatR;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductRequest(
    string? Name = null,
    string? Description = null,
    List<string>? Categories = null,
    string? ImageUrl = null,
    decimal? Price = null);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch(ProductRouteConstant.PRODUCT_ROUTE + "/{id}", async (
            Guid id,
            UpdateProductRequest request,
            ISender sender,
            CancellationToken ct) =>
        {
            var command = ToCommand(request: request, id: id);
            var result = await sender.Send(request: command, cancellationToken: ct);

            if (result.Completed)
            {
                return Results.NoContent();
            }

            return Results.NotFound();
        })
        .WithTags(ProductRouteConstant.PRODUCT_TAG)
        .WithName(ProductRouteConstant.UPDATE_PRODUCT_NAME)
        .Accepts<UpdateProductRequest>(ProductRouteConstant.JSON_CONTENT_TYPE)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary(ProductRouteConstant.UPDATE_PRODUCT_DESCRIPTION)
        .WithDescription(ProductRouteConstant.UPDATE_PRODUCT_DESCRIPTION);
    }

    private static UpdateProductCommand ToCommand(UpdateProductRequest request, Guid id) =>
        new(
            Id: id,
            Name: request.Name,
            Description: request.Description,
            Categories: request.Categories,
            ImageUrl: request.ImageUrl,
            Price: request.Price);
}
