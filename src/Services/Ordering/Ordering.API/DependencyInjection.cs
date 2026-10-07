using BuildingBlock.Exceptions.Handler;
using Carter;
using Scalar.AspNetCore;

namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Add api service dependencies
        services.AddOpenApi();
        services.AddCarter();

        services.AddExceptionHandler<CustomExceptionHandler>();

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

        return app;
    }
}
