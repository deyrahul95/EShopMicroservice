using BuildingBlock.Exceptions;

namespace Ordering.Application.Exceptions;

public class OrderNotFoundException(Guid orderId) : NotFoundException("Order", orderId.ToString())
{

}
