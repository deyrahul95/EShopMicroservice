using System.Text.Json;
using Basket.Core.Domains;
using BuildingBlock.CQRS;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);


public class GetBasketQueryHandler(ILogger<GetBasketQueryHandler> logger)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken ct)
    {
        logger.LogInformation(
            "Executing get basket query: {@Query}",
            query);
        // TODO: Get basket from the database using repository pattern

        await Task.Delay(10, ct);
        var result = new GetBasketResult(new ShoppingCart("dey_rahul"));

        logger.LogInformation(
            "Completed get basket query. Result: {@Result}",
            JsonSerializer.Serialize(result));
        return result;
    }
}
