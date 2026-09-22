using Basket.Core.Domains;
using Basket.Core.Repositories;
using BuildingBlock.Behaviors;
using BuildingBlock.Exceptions.Handler;
using Carter;
using Discount.Grpc;
using FluentValidation;
using HealthChecks.UI.Client;
using Marten;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;
var databaseConnection = builder.Configuration.GetConnectionString("Database")
    ?? throw new Exception("Database connection string can't be empty.");
var redisConnection = builder.Configuration.GetConnectionString("Redis")
    ?? throw new Exception("Redis connection string can't be empty.");

// Add services to the container
#region Application Service
builder.Services.AddOpenApi();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();
#endregion

#region Data Service
builder.Services.AddMarten(options =>
{
    options.Connection(databaseConnection);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = redisConnection;
});

builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
#endregion

#region Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    var discountUrl = builder.Configuration["GrpcSettings:DiscountUrl"]
        ?? throw new Exception("Grpc Discount Url can't be null or empty.");
    options.Address = new Uri(discountUrl);
});
#endregion

#region Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(databaseConnection)
    .AddRedis(redisConnection);
#endregion

var app = builder.Build();

// Configured the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();

app.UseExceptionHandler(option => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
