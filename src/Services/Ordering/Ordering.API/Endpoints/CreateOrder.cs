using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.API.Constants;
using Ordering.Application.Dtos;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.API.Endpoints;

public record CreateOrderRequest(OrderDto Order);

public record CreateOrderResponse(Guid Id);

public class CreateOrder : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            OrderRouteConstant.ORDER_ROUTE,
            async (
                [FromBody] CreateOrderRequest request,
                ISender sender,
                CancellationToken ct = default) =>
            {
                var command = new CreateOrderCommand(request.Order);
                var result = await sender.Send(command, ct);
                var response = new CreateOrderResponse(result.Id);
                return TypedResults.Created($"{OrderRouteConstant.ORDER_ROUTE}/{response.Id}", response);
            })
            .WithTags(OrderRouteConstant.ORDER_TAG)
            .WithName(OrderRouteConstant.CREATE_ORDER_NAME)
            .Accepts<CreateOrderRequest>(OrderRouteConstant.JSON_CONTENT_TYPE)
            .Produces<CreateOrderResponse>(
            StatusCodes.Status201Created,
            OrderRouteConstant.JSON_CONTENT_TYPE)
            .ProducesProblem(
            StatusCodes.Status400BadRequest,
            OrderRouteConstant.JSON_CONTENT_TYPE)
            .WithSummary(OrderRouteConstant.CREATE_ORDER_DESCRIPTION)
            .WithDescription(OrderRouteConstant.CREATE_ORDER_DESCRIPTION);
    }
}
