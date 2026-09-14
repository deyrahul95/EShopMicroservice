using Basket.Core.Constants;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Basket.DeleteBasket;

public class DeleteBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            BasketRouteConstant.BASKET_ROUTE_V1 + "/{username}",
            async ([FromRoute] string username, ISender sender, CancellationToken ct = default) =>
            {
                var result = await sender.Send(
                    new DeleteBasketCommand(username),
                    ct);
                return Results.NoContent();
            })
            .WithTags(BasketRouteConstant.BASKET_TAG)
            .Produces(StatusCodes.Status204NoContent)
            .ProducesProblem(StatusCodes.Status400BadRequest, BasketRouteConstant.JSON_CONTENT_TYPE)
            .WithName(BasketRouteConstant.DELETE_BASKET_NAME)
            .WithSummary(BasketRouteConstant.DELETE_BASKET_DESCRIPTION)
            .WithDescription(BasketRouteConstant.DELETE_BASKET_DESCRIPTION);
    }
}
