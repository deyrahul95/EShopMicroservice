using Basket.API.Models;
using Basket.Core.Domains;

namespace Basket.API.Extensions;

public static class ShoppingCartExtension
{
    public static ShoppingCartItemDto ToDto(this ShoppingCartItem cartItem)
        => new(
            Quantity: cartItem.Quantity,
            Color: cartItem.Color,
            Price: cartItem.Price,
            ProductId: cartItem.ProductId,
            ProductName: cartItem.ProductName);

    public static IReadOnlyList<ShoppingCartItemDto> ToDtoList(this List<ShoppingCartItem> cartItems)
        => [.. cartItems.Select(ToDto)];

    public static ShoppingCartDto ToDto(this ShoppingCart cart)
        => new(
            UserName: cart.UserName,
            Items: cart.Items.ToDtoList(),
            LastModified: cart.LastModified,
            TotalPrice: cart.TotalPrice);

    public static IReadOnlyList<ShoppingCartDto> ToDtoList(this List<ShoppingCart> carts)
        => [.. carts.Select(ToDto)];
}
