using BuildingBlock.CQRS;
using Microsoft.Extensions.Logging;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder;

public class CreateOrderHandler(IAppDbContext dbContext, ILogger<CreateOrderHandler> logger)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken ct = default)
    {
        logger.LogInformation("Executing create order command: {@Command}", command);
        var order = CreateNewOrder(command.Order);

        logger.LogInformation("Saving order {@OrderId} into database.", order.Id.Value);
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(ct);
        logger.LogInformation("Order {@OrderId} saved into database successfully.", order.Id.Value);

        var result = new CreateOrderResult(order.Id.Value);
        logger.LogInformation("Create order command executed successfully. Result: {@Result}", result);

        return result;
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        logger.LogInformation("Initializing order from order dto");
        var newOrder = Order.Create(
            id: OrderId.Of(Guid.NewGuid()),
            customerId: CustomerId.Of(orderDto.CustomerId),
            orderName: OrderName.Of(orderDto.OrderName),
            shippingAddress: orderDto.ShippingAddress.ToAddress(),
            billingAddress: orderDto.BillingAddress.ToAddress(),
            payment: orderDto.Payment.ToPayment()
        );

        logger.LogInformation("Order {@OrderId} initialized from order dto.", newOrder.Id.Value);

        foreach (var orderItemDto in orderDto.OrderItems)
        {
            newOrder.Add(
                productId: ProductId.Of(orderItemDto.ProductId),
                quantity: orderItemDto.Quantity,
                price: orderItemDto.Price
            );
            logger.LogInformation("OrderItem with product id: {@ProductId} has been added into order", orderItemDto.ProductId);
        }

        return newOrder;
    }
}
