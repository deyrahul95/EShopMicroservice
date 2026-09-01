using System.Text.Json;
using Basket.Core.Domains;
using BuildingBlock.CQRS;
using FluentValidation;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(string UserName, List<ShoppingCartItem> CartItems) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Username is required")
            .MaximumLength(100)
            .WithMessage("User name cannot exceed 100 characters.");

        RuleFor(x => x.CartItems)
            .NotNull()
            .WithMessage("Cart items are required.")
            .NotEmpty()
            .WithMessage("The basket must contain at least one item.");

        RuleForEach(x => x.CartItems)
            .SetValidator(new ShoppingCartItemValidator());
    }
}

public class ShoppingCartItemValidator : AbstractValidator<ShoppingCartItem>
{
    public ShoppingCartItemValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Color)
            .NotNull()
            .NotEmpty()
            .WithMessage("Color is required.")
            .MaximumLength(50)
            .WithMessage("Color cannot exceed 50 characters.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");

        RuleFor(x => x.ProductId)
            .NotEqual(Guid.Empty)
            .WithMessage("Product ID must be a valid GUID.");

        RuleFor(x => x.ProductName)
            .NotNull()
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(200)
            .WithMessage("Product name cannot exceed 200 characters.");
    }
}

public class StoreBasketCommandHandler(ILogger<StoreBasketCommandHandler> logger)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken ct)
    {
        logger.LogInformation(
            "Executing store basket command: {@Command}",
            JsonSerializer.Serialize(command));
        await Task.Delay(10, ct);

        var cart = new ShoppingCart(command.UserName)
        {
            Items = command.CartItems,
            LastModified = DateTime.UtcNow
        };

        logger.LogInformation(
            "Cart store successfully: {@Cart}",
            JsonSerializer.Serialize(cart));

        // TODO: store basket in database
        var result = new StoreBasketResult(cart.UserName);

        logger.LogInformation(
            "Completed store basket command: {@Result}",
            result);
        return result;
    }
}
