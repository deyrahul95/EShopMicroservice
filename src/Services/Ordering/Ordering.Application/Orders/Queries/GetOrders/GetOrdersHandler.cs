using System.Text.Json;
using BuildingBlock.CQRS;
using BuildingBlock.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Application.Data;
using Ordering.Application.Dtos;
using Ordering.Application.Extensions;

namespace Ordering.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler(
    IAppDbContext dbContext,
    ILogger<GetOrdersHandler> logger)
    : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Executing get orders query: {@Query}",
            query);

        logger.LogInformation("Fetching orders count from database");
        var totalCount = await dbContext.Orders
            .AsNoTracking()
            .LongCountAsync(ct);
        logger.LogInformation("Fetched orders count: {@Count} from database", totalCount);

        var pageNumber = query.PaginationRequest.PageNumber;
        var pageSize = query.PaginationRequest.PageSize;

        logger.LogInformation(
            "Fetching orders from database for page number: {@PageNumber} and page size: {@PageSize}",
            pageNumber,
            pageSize);
        var orders = await dbContext.Orders
            .Include(o => o.OrderItems)
            .AsNoTracking()
            .OrderBy(o => o.OrderName.Value)
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize)
            .ToListAsync(ct);
        logger.LogInformation(
            "Fetched {@Count} orders data from database",
            orders.Count);

        var result = new GetOrdersResult(
            Orders: new PaginatedResult<OrderDto>(
                pageNumber: pageNumber,
                pageSize: pageSize,
                totalCount: totalCount,
                data: orders.ToDtoList()));
        logger.LogInformation(
            "Executed get orders query with result: {@Result}",
            JsonSerializer.Serialize(result));

        return result;
    }
}
