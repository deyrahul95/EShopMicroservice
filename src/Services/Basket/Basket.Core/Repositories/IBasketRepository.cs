using Basket.Core.Domains;

namespace Basket.Core.Repositories;

public interface IBasketRepository
{
    Task<ShoppingCart> GetBasket(string userName, CancellationToken ct = default);
    Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken ct = default);
    Task<bool> DeleteBasket(string userName, CancellationToken ct = default);
}
