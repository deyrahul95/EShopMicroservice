using BuildingBlock.CQRS;
using Ordering.Application.Dtos;

namespace Ordering.Application.Orders.Queries.GetOrderByName;

public record GetOrdersByNameQuery(string Name) : IQuery<GetOrderByNameResult>;
public record GetOrderByNameResult(IEnumerable<OrderDto> Orders);
