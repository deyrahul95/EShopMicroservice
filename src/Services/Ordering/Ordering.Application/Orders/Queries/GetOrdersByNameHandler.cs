using System.Text.Json;
using BuildingBlock.CQRS;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Data;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries;

public class GetOrdersByNameHandler(IAppDbContext dbContext, ILogger<GetOrdersByNameHandler> logger)
    : IQueryHandler<GetOrdersByNameQuery, GetOrderByNameResult>
{
    public async Task<GetOrderByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken ct = default)
    {
        logger.LogInformation("Executing get order by name query: {@Query}", query);

        logger.LogInformation("Fetching orders from database with name: {@OrderName}", query.Name);
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .Where(o => o.OrderName.Value.ToLower().Contains(query.Name.ToLower()))
            .OrderBy(o => o.OrderName)
            .ToListAsync(ct);
        logger.LogInformation("Found {@Count} orders with name: {@OrderName}", orders.Count, query.Name);

        var result = new GetOrderByNameResult(orders.ToDtoList());
        logger.LogInformation(
            message: "Executed get order by name query. Result: {@Result}",
            args: JsonSerializer.Serialize(result));

        return result;
    }
}