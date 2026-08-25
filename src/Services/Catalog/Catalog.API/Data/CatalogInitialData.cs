using Catalog.API.Models;
using Marten;
using Marten.Schema;

namespace Catalog.API.Data;

public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken ct)
    {
        using var session = store.LightweightSession();

        // If already products exists in database, do nothing
        if (await session.Query<Product>().AnyAsync(ct))
        {
            return;
        }

        // Marten UPSERT will cater for existing records
        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync(ct);
    }

    private static IEnumerable<Product> GetPreconfiguredProducts() => [
        new Product{
            Id = new Guid("01a036d4-b970-7367-ad0c-23b7c1104200"),
            Name = "Samsung Galaxy S26 Ultra 5G",
            Category = [
                "Smartphone",
                "AI",
                "5G"
            ],
            Description= "Samsung Galaxy S26 Ultra 5G (Cobalt Violet, 12GB RAM, 256GB Storage) with Built-in Privacy Display, AI Phone, Photo Assist, Creative Studio, 200MP Camera, 5000mAh Battery and Snapdragon 8 Elite Gen 5",
            ImageUrl = "https://m.media-amazon.com/images/G/31/apparel/rcxgs/tile._CB483369979_.gif",
            Price= 124999
        },
        new Product{
            Id = new Guid("01a036d2-5410-7363-996e-cf355c655baa"),
            Name= "Samsung Galaxy S25 Ultra 5G",
            Category = [
                "Smartphone",
                "Electronics",
                "AI",
                "5G"
            ],
            Description= "Samsung Galaxy S25 Ultra 5G AI Smartphone (Titanium Gray, 12GB RAM, 256GB Storage), 200MP Camera, S Pen Included, Long Battery Life ",
            ImageUrl = "https://m.media-amazon.com/images/I/71NHyfz-isL._SX679_.jpg",
            Price = 99999,
        }
    ];
}
