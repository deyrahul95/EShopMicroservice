namespace Catalog.API.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageUrl,
    decimal Price);

public record CreateProductResult(Guid Id);

public class CreateProductHandler
{
    public Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
