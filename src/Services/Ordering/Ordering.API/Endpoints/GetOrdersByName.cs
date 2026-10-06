using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Constants;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrderByName;

namespace Ordering.API.Endpoints;

public record GetOrdersByNameResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByName : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            OrderRouteConstant.GET_ORDERS_BY_NAME_ROUTE,
            async ([FromRoute] string orderName, ISender sender, CancellationToken ct = default) =>
            {
                var query = new GetOrdersByNameQuery(Name: orderName);
                var result = await sender.Send(request: query, cancellationToken: ct);
                var response = new GetOrderByNameResult(result.Orders);
                return TypedResults.Ok(response);
            })
            .WithTags(OrderRouteConstant.ORDER_TAG)
            .WithName(OrderRouteConstant.GET_ORDERS_BY_NAME_ROUTE_NAME)
            .Produces<GetOrdersByNameResponse>(StatusCodes.Status200OK, OrderRouteConstant.JSON_CONTENT_TYPE)
            .WithSummary(OrderRouteConstant.GET_ORDERS_BY_NAME_DESCRIPTION)
            .WithDescription(OrderRouteConstant.GET_ORDERS_BY_NAME_DESCRIPTION);
    }
}
