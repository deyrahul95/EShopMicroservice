using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public static class DbExtensions
{
    public static async Task<IApplicationBuilder> ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DiscountDbContext>();
        await dbContext.Database.MigrateAsync();

        return app;
    }
}
