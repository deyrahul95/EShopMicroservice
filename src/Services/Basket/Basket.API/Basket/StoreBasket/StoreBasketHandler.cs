using System.Text.Json;
using Basket.Core.Domains;
using Basket.Core.Repositories;
using BuildingBlock.CQRS;
using Discount.Grpc;
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

public class StoreBasketCommandHandler(
    IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProtoService,
    ILogger<StoreBasketCommandHandler> logger) : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken ct)
    {
        logger.LogInformation(
            "Executing store basket command: {@Command}",
            JsonSerializer.Serialize(command));

        var cart = new ShoppingCart(command.UserName)
        {
            Items = command.CartItems,
            LastModified = DateTime.UtcNow
        };

        // Checking if any discount is applied for the items 
        await DeductDiscount(cart: cart, ct: ct);

        cart = await repository.StoreBasket(cart, ct);
        logger.LogInformation(
            "Cart store successfully: {@Cart}",
            JsonSerializer.Serialize(cart));

        var result = new StoreBasketResult(cart.UserName);

        logger.LogInformation(
            "Completed store basket command: {@Result}",
            result);
        return result;
    }

    private async Task DeductDiscount(ShoppingCart cart, CancellationToken ct)
    {
        foreach (var item in cart.Items)
        {
            logger.LogInformation("Checking discount for product {@ProductName}", item.ProductName);
            var discountRequest = new GetDiscountRequest { ProductName = item.ProductName };

            logger.LogInformation("Sending get discount request {@Request}", discountRequest);
            var coupon = await discountProtoService.GetDiscountAsync(request: discountRequest, cancellationToken: ct);
            logger.LogInformation("Received get discount response {@Response}", JsonSerializer.Serialize(coupon));

            if (coupon.Id != 0 && coupon.Amount > 0)
            {
                logger.LogInformation(
                    "Found discount for product {@ProductName} amount {@Amount}",
                    coupon.ProductName,
                    coupon.Amount);
                item.Price -= coupon.Amount;
                logger.LogInformation("Discount applied for product with id {@ProductId}", item.ProductId);
            }
        }
    }
}
