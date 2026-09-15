using System.Text.Json;
using Basket.Core.Domains;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace Basket.Core.Repositories;

public class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache,
    ILogger<CachedBasketRepository> logger) : IBasketRepository
{
    public async Task<ShoppingCart> GetBasket(string userName, CancellationToken ct = default)
    {
        logger.LogInformation("Retrieving {@UserName} basket details from cached.", userName);
        var cachedBasket = await cache.GetStringAsync(userName, ct);

        if (string.IsNullOrEmpty(cachedBasket) == false)
        {
            logger.LogInformation(
                "Retrieved {@UserName} basket detailed from cached. Basket: {@Basket}",
                userName,
                cachedBasket);
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)
                ?? throw new Exception("Failed to deserialized cached basket data.");
        }

        logger.LogInformation("Cache missed for {@UserName}", userName);

        var basket = await repository.GetBasket(userName, ct);
        await cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), ct);
        logger.LogInformation("Cache set for {@UserName}", userName);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken ct = default)
    {
        var updatedBasket = await repository.StoreBasket(basket, ct);
        await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), ct);
        logger.LogInformation("Cache set for {@UserName}", basket.UserName);

        return updatedBasket;
    }

    public async Task<bool> DeleteBasket(string userName, CancellationToken ct = default)
    {
        var isDeleted = await repository.DeleteBasket(userName, ct);

        if (isDeleted)
        {
            await cache.RemoveAsync(userName, ct);
            logger.LogInformation("Cache removed for {@UserName}", userName);
        }

        return isDeleted;
    }
}
