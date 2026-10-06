using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Constants;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrdersByCustomer;

namespace Ordering.API.Endpoints;

public record GetOrdersByCustomerResponse(IEnumerable<OrderDto> Orders);

public class GetOrdersByCustomer : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            OrderRouteConstant.GET_ORDERS_BY_CUSTOMER_ROUTE,
            async ([FromRoute] Guid customerId, ISender sender, CancellationToken ct = default) =>
            {
                var query = new GetOrdersByCustomerQuery(customerId);
                var result = await sender.Send(query, ct);
                var response = new GetOrdersByCustomerResponse(result.Orders);
                return TypedResults.Ok(response);
            })
            .WithTags(OrderRouteConstant.ORDER_TAG)
            .WithName(OrderRouteConstant.GET_ORDERS_BY_CUSTOMER_NAME)
            .Produces<GetOrdersByCustomerResult>(StatusCodes.Status200OK, OrderRouteConstant.JSON_CONTENT_TYPE)
            .WithSummary(OrderRouteConstant.GET_ORDERS_BY_CUSTOMER_DESCRIPTION)
            .WithDescription(OrderRouteConstant.GET_ORDERS_BY_CUSTOMER_DESCRIPTION);
    }
}
