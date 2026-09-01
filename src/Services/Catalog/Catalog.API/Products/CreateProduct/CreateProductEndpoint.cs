using Carter;
using Catalog.API.Constants;
using MediatR;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductRequest(string Name,
    List<string> Categories,
    string Description,
    string ImageUrl,
    decimal Price);

public record CreateProductResponse(Guid Id);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(ProductRouteConstant.PRODUCT_ROUTE,
            async (CreateProductRequest request, ISender sender, CancellationToken ct) =>
        {
            var command = ToCommand(request);
            var result = await sender.Send(command, ct);
            var response = ToResponse(result);
            return Results.Created($"{ProductRouteConstant.PRODUCT_ROUTE}/{response.Id}", response);
        })
        .WithTags(ProductRouteConstant.PRODUCT_TAG)
        .WithName(ProductRouteConstant.CREATE_PRODUCT_NAME)
        .Accepts<CreateProductRequest>(ProductRouteConstant.JSON_CONTENT_TYPE)
        .Produces<CreateProductResponse>(StatusCodes.Status201Created, ProductRouteConstant.JSON_CONTENT_TYPE)
        .ProducesProblem(StatusCodes.Status400BadRequest, ProductRouteConstant.JSON_CONTENT_TYPE)
        .WithSummary(ProductRouteConstant.CREATE_PRODUCT_DESCRIPTION)
        .WithDescription(ProductRouteConstant.CREATE_PRODUCT_DESCRIPTION);
    }

    private static CreateProductCommand ToCommand(CreateProductRequest request)
        => new(
            Name: request.Name,
            Categories: request.Categories,
            Description: request.Description,
            ImageUrl: request.ImageUrl,
            Price: request.Price);

    private static CreateProductResponse ToResponse(CreateProductResult result)
        => new(result.Id);
}
