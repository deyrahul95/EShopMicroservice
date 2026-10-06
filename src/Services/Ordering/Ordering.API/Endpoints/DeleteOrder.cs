using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Constants;
using Ordering.Application.Orders.Commands.DeleteOrder;

namespace Ordering.API.Endpoints;

public record DeleteOrderResponse(bool IsSuccess);

public class DeleteOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            OrderRouteConstant.DELETE_ORDER_ROUTE,
            async ([FromRoute] Guid id, ISender sender, CancellationToken ct = default) =>
            {
                var command = new DeleteOrderCommand(id);
                var result = await sender.Send(command, ct);
                var response = new DeleteOrderResponse(result.IsSuccess);
                return TypedResults.Ok(response);
            })
            .WithTags(OrderRouteConstant.ORDER_TAG)
            .WithName(OrderRouteConstant.DELETE_ORDER_NAME)
            .Produces<DeleteOrderResponse>(StatusCodes.Status200OK, OrderRouteConstant.JSON_CONTENT_TYPE)
            .ProducesProblem(StatusCodes.Status404NotFound, OrderRouteConstant.JSON_CONTENT_TYPE)
            .WithSummary(OrderRouteConstant.DELETE_ORDER_DESCRIPTION)
            .WithDescription(OrderRouteConstant.DELETE_ORDER_DESCRIPTION);
    }
}
