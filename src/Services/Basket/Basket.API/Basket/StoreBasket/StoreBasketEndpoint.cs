using Basket.API.Models;
using Basket.Core.Constants;
using Basket.Core.Domains;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(List<ShoppingCartItemDto> Items);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            BasketRouteConstant.BASKET_ROUTE_V1 + "/{username}",
            async (
                [FromRoute] string userName,
                [FromBody] StoreBasketRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = ToCommand(userName: userName, request: request);
                var result = await sender.Send(command, ct);
                return Results.NoContent();
            })
        .WithTags(BasketRouteConstant.BASKET_TAG)
        .WithName(BasketRouteConstant.STORE_BASKET_NAME)
        .Accepts<StoreBasketRequest>(BasketRouteConstant.JSON_CONTENT_TYPE)
        .Produces(StatusCodes.Status204NoContent)
        .ProducesProblem(StatusCodes.Status400BadRequest, BasketRouteConstant.JSON_CONTENT_TYPE)
        .WithDescription(BasketRouteConstant.STORE_BASKET_DESCRIPTION)
        .WithSummary(BasketRouteConstant.STORE_BASKET_DESCRIPTION);
    }

    private static StoreBasketCommand ToCommand(string userName, StoreBasketRequest request)
        => new(
            UserName: userName,
            CartItems: [.. request.Items.Select(
                x => new ShoppingCartItem(){
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    Color = x.Color,
                    Quantity = x.Quantity
                })]);
}
