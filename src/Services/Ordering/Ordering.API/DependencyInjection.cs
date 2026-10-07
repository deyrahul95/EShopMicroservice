using BuildingBlock.Exceptions.Handler;
using Carter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Add api service dependencies
        var databaseConnection = configuration.GetConnectionString("Database")
            ?? throw new Exception("Database connection string can't be empty!");

        services.AddOpenApi();
        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();
        services.AddHealthChecks()
            .AddSqlServer(databaseConnection);

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Map api services
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.MapCarter();

        app.UseExceptionHandler(option => { });
        app.UseHealthChecks("/api/health", new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

        return app;
    }
}
