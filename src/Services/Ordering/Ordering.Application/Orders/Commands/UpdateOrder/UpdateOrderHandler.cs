using BuildingBlock.CQRS;
using Microsoft.Extensions.Logging;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Exceptions;
using Ordering.Application.Extensions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.UpdateOrder;

public class UpdateOrderHandler(IAppDbContext dbContext, ILogger<UpdateOrderHandler> logger)
    : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken ct = default)
    {
        logger.LogInformation("Executing update order command: {@Command}", command);

        var orderId = OrderId.Of(command.Order.Id);

        logger.LogInformation("Fetching orders for id: {@OrderId}", orderId.Value);
        var order = await dbContext.Orders.FindAsync([orderId], ct);

        if (order is null)
        {
            logger.LogInformation("No order found with id: {@OrderId}", orderId.Value);
            throw new OrderNotFoundException(command.Order.Id);
        }

        UpdateExistingOrder(order, command.Order);

        logger.LogInformation("Updating order {@OrderId} into database.", order.Id.Value);
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(ct);
        logger.LogInformation("Order {@OrderId} updated into database successfully.", order.Id.Value);

        var result = new UpdateOrderResult(IsSuccess: true);
        logger.LogInformation("Update order command executed successfully. Result: {@Result}", result);

        return result;
    }

    private void UpdateExistingOrder(Order existingOrder, OrderDto orderDto)
    {
        logger.LogInformation("Updating order {@OrderId} properties", existingOrder.Id.Value);
        existingOrder.Update(
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: orderDto.ShippingAddress.ToAddress(),
            billingAddress: orderDto.BillingAddress.ToAddress(),
            payment: orderDto.Payment.ToPayment(),
            orderStatus: orderDto.OrderStatus
        );

        logger.LogInformation("Order {@OrderId} properties got updated", existingOrder.Id.Value);
    }
}