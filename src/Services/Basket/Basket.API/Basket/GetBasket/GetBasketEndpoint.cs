using Basket.API.Extensions;
using Basket.API.Models;
using Basket.Core.Constants;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.GetBasket;

// public record GetBasketRequest(string UserName);
public record GetBasketResponse(ShoppingCartDto Cart);

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            BasketRouteConstant.BASKET_ROUTE_V1 + "/{username}",
            async ([FromRoute] string userName, ISender sender, CancellationToken ct) =>
            {
                var query = new GetBasketQuery(userName);
                var result = await sender.Send(query, ct);
                return Results.Ok(ToResponse(result));
            })
            .WithTags(BasketRouteConstant.BASKET_TAG)
            .WithName(BasketRouteConstant.GET_BASKETS_NAME)
            .Produces<GetBasketResponse>(StatusCodes.Status200OK, BasketRouteConstant.JSON_CONTENT_TYPE)
            .ProducesProblem(StatusCodes.Status500InternalServerError, BasketRouteConstant.JSON_CONTENT_TYPE)
            .WithDescription(BasketRouteConstant.GET_BASKETS_DESCRIPTION)
            .WithSummary(BasketRouteConstant.GET_BASKETS_DESCRIPTION);
    }

    private static GetBasketResponse ToResponse(GetBasketResult result)
        => new(result.Cart.ToDto());
}
