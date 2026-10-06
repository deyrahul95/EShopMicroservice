using BuildingBlock.CQRS;
using Microsoft.Extensions.Logging;
using Ordering.Application.Data;
using Ordering.Application.Exceptions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IAppDbContext dbContext, ILogger<DeleteOrderHandler> logger)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken ct = default)
    {
        logger.LogInformation("Executing delete order command: {@Command}", command);

        var orderId = OrderId.Of(command.OrderId);

        logger.LogInformation("Fetching orders for id: {@OrderId}", orderId.Value);
        var order = await dbContext.Orders.FindAsync([orderId], ct);

        if (order is null)
        {
            logger.LogInformation("No order found with id: {@OrderId}", orderId.Value);
            throw new OrderNotFoundException(command.OrderId);
        }

        logger.LogInformation("Deleting {@OrderId} from database", orderId.Value);
        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(ct);
        logger.LogInformation("Order {@OrderId} deleted from database", orderId.Value);

        var result = new DeleteOrderResult(IsSuccess: true);
        logger.LogInformation("Delete order command executed successfully. Result: {@Result}", result);

        return result;
    }
}
