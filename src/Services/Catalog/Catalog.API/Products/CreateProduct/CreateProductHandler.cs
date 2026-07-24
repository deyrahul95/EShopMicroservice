using System.Text.Json;
using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageUrl,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

internal class CreateProductCommandHandler(
    IDocumentSession session,
    ILogger<CreateProductCommandHandler> logger)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken ct)
    {
        logger.LogInformation("Start executing create product command handler. Command: {@Command}", command);

        // Create product entity from the command object
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            Category = command.Categories,
            ImageUrl = command.ImageUrl,
            Price = command.Price
        };

        // Save this product into database
        session.Store(product);
        await session.SaveChangesAsync(ct);

        logger.LogInformation("Product with id {@ProductId} created successfully.", product.Id);
        logger.LogInformation("Completed create product command handler. Product: {@Product}", JsonSerializer.Serialize(product));
        // Return the result
        return new CreateProductResult(product.Id);
    }
}
