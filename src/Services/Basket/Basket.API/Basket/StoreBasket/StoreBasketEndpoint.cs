using Basket.API.Models;
using Basket.Core.Constants;
using Basket.Core.Domains;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.StoreBasket;

public record StoreBasketRequest(string UserName, List<ShoppingCartItemDto> Items);
public record StoreBasketResponse(string UserName);

public class StoreBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            BasketRouteConstant.BASKET_ROUTE_V1,
            async (
                [FromBody] StoreBasketRequest request,
                ISender sender,
                CancellationToken ct) =>
            {
                var command = ToCommand(request: request);
                var result = await sender.Send(command, ct);
                var response = new StoreBasketResponse(UserName: result.UserName);
                return Results.Created($"{BasketRouteConstant.BASKET_ROUTE_V1}/{response.UserName}", response);
            })
        .WithTags(BasketRouteConstant.BASKET_TAG)
        .WithName(BasketRouteConstant.STORE_BASKET_NAME)
        .Accepts<StoreBasketRequest>(BasketRouteConstant.JSON_CONTENT_TYPE)
        .Produces<StoreBasketResponse>(StatusCodes.Status201Created, BasketRouteConstant.JSON_CONTENT_TYPE)
        .ProducesProblem(StatusCodes.Status400BadRequest, BasketRouteConstant.JSON_CONTENT_TYPE)
        .WithDescription(BasketRouteConstant.STORE_BASKET_DESCRIPTION)
        .WithSummary(BasketRouteConstant.STORE_BASKET_DESCRIPTION);
    }

    private static StoreBasketCommand ToCommand(StoreBasketRequest request)
        => new(
            UserName: request.UserName,
            CartItems: [.. request.Items.Select(
                x => new ShoppingCartItem(){
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    Color = x.Color,
                    Quantity = x.Quantity
                })]);
}
