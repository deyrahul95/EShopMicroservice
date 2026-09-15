using System.Text.Json;
using Basket.Core.Domains;
using Basket.Core.Repositories;
using BuildingBlock.CQRS;

namespace Basket.API.Basket.GetBasket;

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;
public record GetBasketResult(ShoppingCart Cart);


public class GetBasketQueryHandler(IBasketRepository repository, ILogger<GetBasketQueryHandler> logger)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    public async Task<GetBasketResult> Handle(GetBasketQuery query, CancellationToken ct)
    {
        logger.LogInformation(
            "Executing get basket query: {@Query}",
            query);
        var basket = await repository.GetBasket(query.UserName, ct);

        var result = new GetBasketResult(basket);

        logger.LogInformation(
            "Completed get basket query. Result: {@Result}",
            JsonSerializer.Serialize(result));
        return result;
    }
}
