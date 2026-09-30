using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Data;
using Ordering.Infrastructure.Data.Interceptors;

namespace Ordering.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add infrastructure service dependencies
        var dbConnection = configuration.GetConnectionString("Database");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.AddInterceptors(new AuditableEntityInterceptor());
            options.UseSqlServer(dbConnection);
        });

        return services;
    }
}
