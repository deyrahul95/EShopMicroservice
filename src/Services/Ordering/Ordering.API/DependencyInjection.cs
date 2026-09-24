namespace Ordering.API;

public static class DependencyInjection
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        // Add api service dependencies

        return services;
    }

    public static WebApplication UseApiServices(this WebApplication app)
    {
        // Map api services

        return app;
    }
}
