using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Constants;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.UpdateOrder;

namespace Ordering.API.Endpoints;

public record UpdateOrderRequest(OrderDto Order);
public record UpdateOrderResponse(bool IsSuccess);

public class UpdateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            OrderRouteConstant.ORDER_ROUTE,
            async (
                [FromBody] UpdateOrderRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new UpdateOrderCommand(request.Order);
                var result = await sender.Send(command, ct);
                var response = new UpdateOrderResponse(result.IsSuccess);
                return TypedResults.Ok(response);
            })
            .WithTags(OrderRouteConstant.ORDER_TAG)
            .WithName(OrderRouteConstant.UPDATE_ORDER_NAME)
            .Accepts<UpdateOrderRequest>(OrderRouteConstant.JSON_CONTENT_TYPE)
            .Produces<UpdateOrderResponse>(StatusCodes.Status200OK, OrderRouteConstant.JSON_CONTENT_TYPE)
            .ProducesProblem(StatusCodes.Status404NotFound, OrderRouteConstant.JSON_CONTENT_TYPE)
            .ProducesProblem(StatusCodes.Status400BadRequest, OrderRouteConstant.JSON_CONTENT_TYPE)
            .WithSummary(OrderRouteConstant.UPDATE_ORDER_DESCRIPTION)
            .WithDescription(OrderRouteConstant.CREATE_ORDER_DESCRIPTION);
    }
}
