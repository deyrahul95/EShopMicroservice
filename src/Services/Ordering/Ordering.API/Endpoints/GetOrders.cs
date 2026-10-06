using BuildingBlock.Pagination;
using Carter;
using MediatR;
using Ordering.API.Constants;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Queries.GetOrders;

namespace Ordering.API.Endpoints;

public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);

public class GetOrders : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            OrderRouteConstant.ORDER_ROUTE,
            async ([AsParameters] PaginationRequest request, ISender sender, CancellationToken ct = default) =>
            {
                var query = new GetOrdersQuery(request);
                var result = await sender.Send(query, ct);
                var response = new GetOrdersResponse(result.Orders);
                return TypedResults.Ok(response);
            })
            .WithTags(OrderRouteConstant.ORDER_TAG)
            .WithName(OrderRouteConstant.GET_ORDERS_NAME)
            .Produces<GetOrdersResponse>(StatusCodes.Status200OK, OrderRouteConstant.JSON_CONTENT_TYPE)
            .WithSummary(OrderRouteConstant.GET_ORDERS_DESCRIPTION)
            .WithDescription(OrderRouteConstant.GET_ORDERS_DESCRIPTION);
    }
}
