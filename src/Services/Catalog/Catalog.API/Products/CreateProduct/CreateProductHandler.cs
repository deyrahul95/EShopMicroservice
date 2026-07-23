using BuildingBlock.CQRS;
using Catalog.API.Models;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageUrl,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken ct)
    {
        // Create product entity from the command object
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Category = command.Categories,
            ImageUrl = command.ImageUrl,
            Price = command.Price
        };
        // TODO: Save this product into database
        await Task.Delay(200, ct);

        // Return the result
        return new CreateProductResult(product.Id);
    }
}
