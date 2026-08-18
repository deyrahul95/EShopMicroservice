using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string? Name = null,
    string? Description = null,
    List<string>? Category = null,
    string? ImageUrl = null,
    decimal? Price = null) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool Completed);

internal class UpdateProductCommandHandler(
    IDocumentSession session,
    ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        logger.LogInformation(
            "Start executing update product command handler. Command: {@Command}",
            command);

        var product = await session.LoadAsync<Product>(command.Id, ct);

        if (product is null)
        {
            logger.LogInformation("No product found with id :{@Id} in our database.", command.Id);
            return new UpdateProductResult(Completed: false);
        }

        product.Name = string.IsNullOrEmpty(command.Name) ? product.Name : command.Name;
        product.Description = string.IsNullOrEmpty(command.Description) ? product.Description : command.Description;
        product.Category = command.Category?.Count == 0 ? product.Category : command.Category ?? [];
        product.ImageUrl = string.IsNullOrEmpty(command.ImageUrl) ? product.ImageUrl : command.ImageUrl;
        product.Price = command.Price ?? product.Price;
        product.UpdatedAt = DateTime.UtcNow;

        session.Update(product);
        await session.SaveChangesAsync(ct);

        logger.LogInformation("Product with id {@ProductId} updated successfully.", command.Id);

        logger.LogInformation("Completed updated product command handler. Update Product: {@Product}", product);
        return new UpdateProductResult(Completed: true);
    }
}
