namespace Basket.API.Models;

public record ShoppingCartDto(
    string UserName,
    IReadOnlyList<ShoppingCartItemDto> Items,
    DateTime LastModified,
    decimal TotalPrice);

public record ShoppingCartItemDto(
    int Quantity,
    string Color,
    decimal Price,
    Guid ProductId,
    string ProductName);
