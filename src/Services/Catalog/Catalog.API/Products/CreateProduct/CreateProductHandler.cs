using System.Text.Json;
using BuildingBlock.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageUrl,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.Categories)
            .NotNull()
            .NotEmpty()
            .ForEach(item => item.NotEmpty()
                .WithMessage("Categories value cannot be empty"));

        RuleFor(x => x.Categories)
            .Must(categories =>
                categories.Any(c => !string.IsNullOrWhiteSpace(c)))
            .WithMessage("Categories must contain at least one valid, non-empty value");

        RuleFor(x => x.Description)
            .NotNull()
            .NotEmpty()
            .WithMessage("Description is required");

        RuleFor(x => x.ImageUrl)
            .NotNull()
            .NotEmpty()
            .WithMessage("ImageUrl is required");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than 0");
    }
}

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
        logger.LogInformation(
            "Completed create product command handler. Product: {@Product}",
            JsonSerializer.Serialize(product));

        // Return the result
        return new CreateProductResult(product.Id);
    }
}
