using System.Text.Json;
using BuildingBlock.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Data;
using Ordering.Application.Extensions;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Queries.GetOrdersByCustomer;

public class GetOrdersByCustomerHandler(
    IAppDbContext dbContext,
    ILogger<GetOrdersByCustomerHandler> logger)
    : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(
        GetOrdersByCustomerQuery query,
        CancellationToken ct = default)
    {
        logger.LogInformation(
            "Executing get orders by customer query: {@Query}",
            query);

        logger.LogInformation(
            "Fetching orders from database with customer id: {@CustomerId}",
            query.CustomerId);
        var customerId = CustomerId.Of(query.CustomerId);

        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.CustomerId == customerId)
            .OrderBy(o => o.OrderName.Value)
            .ToListAsync(ct);
        logger.LogInformation(
            "Found {@Count} orders with customer id: {@CustomerId}",
            orders.Count,
            query.CustomerId);

        var result = new GetOrdersByCustomerResult(orders.ToDtoList());
        logger.LogInformation(
            "Executed get orders by customer query with result: {@Result}",
            JsonSerializer.Serialize(result));

        return result;
    }
}