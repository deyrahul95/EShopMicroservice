using BuildingBlock.CQRS;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool Completed);

internal class DeleteProductCommandHandler(
    IDocumentSession session,
    ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        logger.LogInformation("Start executing delete product command handler. Command: {@Command}", command);

        var product = await session.LoadAsync<Product>(command.Id, ct);

        if (product is null)
        {
            logger.LogInformation("No product found with id :{@Id} in our database.", command.Id);
            return new DeleteProductResult(Completed: false);
        }

        session.Delete<Product>(command.Id);
        await session.SaveChangesAsync(ct);

        logger.LogInformation("Product with id {@ProductId} deleted successfully.", command.Id);

        logger.LogInformation("Completed delete product command handler.");
        return new DeleteProductResult(Completed: true);
    }
}
