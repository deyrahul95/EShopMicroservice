using BuildingBlock.Exceptions;

namespace Catalog.API.Exceptions;

public class ProductNotFoundException(Guid Id) : NotFoundException(name: "Product", key: Id.ToString())
{
}
