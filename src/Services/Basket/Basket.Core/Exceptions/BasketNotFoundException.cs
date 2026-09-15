using BuildingBlock.Exceptions;

namespace Basket.Core.Exceptions;

public class BasketNotFoundException(string userName)
    : NotFoundException("Basket", userName)
{
}
