using BuildingBlock.CQRS;
using Catalog.API.Models;
using FluentValidation;
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

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull()
            .NotEmpty()
            .WithMessage("Id is required");

        RuleFor(x => x.Name)
            .Must(name => name == null || !string.IsNullOrWhiteSpace(name))
            .WithMessage("Name cannot be empty or whitespace-only");

        RuleFor(x => x.Description)
            .Must(desc => desc == null || !string.IsNullOrWhiteSpace(desc))
            .WithMessage("Description cannot be empty or whitespace-only");

        RuleFor(x => x.Category)
            .Must(categories => categories == null ||
                  (categories.Count > 0 && categories.Any(c => !string.IsNullOrWhiteSpace(c))))
            .WithMessage("Category must be null or contain at least one valid non-empty value");

        RuleFor(x => x.ImageUrl)
            .Must(url => url == null || !string.IsNullOrWhiteSpace(url))
            .WithMessage("ImageUrl cannot be empty or whitespace-only")
            .Must(url => url == null || Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("ImageUrl must be a valid URL");

        RuleFor(x => x.Price)
            .Must(price => price == null || price > 0)
            .WithMessage("Price must be greater than 0");
    }
}

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
