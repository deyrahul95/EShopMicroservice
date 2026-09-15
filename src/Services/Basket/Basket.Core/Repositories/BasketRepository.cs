using System.Text.Json;
using Basket.Core.Domains;
using Basket.Core.Exceptions;
using Marten;
using Microsoft.Extensions.Logging;

namespace Basket.Core.Repositories;

public class BasketRepository(
    IDocumentSession session,
    ILogger<BasketRepository> logger) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken ct = default)
    {
        logger.LogInformation("Fetching basket for username {@UserName} from database", userName);
        var basket = await session.LoadAsync<ShoppingCart>(userName, ct);

        if (basket is null)
        {
            logger.LogInformation("No basket found for username {@UserName} into database", userName);
            throw new BasketNotFoundException(userName);
        }

        logger.LogInformation(
            "Basket fetched successfully from database. {@Basket}",
            JsonSerializer.Serialize(basket));
        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Storing basket into database. {@Basket}",
            JsonSerializer.Serialize(basket));

        session.Store(basket);
        await session.SaveChangesAsync(ct);

        logger.LogInformation(
            "Basket stored successfully into database. {@Basket}",
            JsonSerializer.Serialize(basket));
        return basket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken ct = default)
    {
        logger.LogInformation(
            "Basket deleting from database for username {@UserName}",
            userName);

        session.Delete<ShoppingCart>(userName);
        await session.SaveChangesAsync(ct);

        logger.LogInformation(
            "Basket for username {@UserName} deleted successfully from database",
            userName);
        return true;
    }
}
