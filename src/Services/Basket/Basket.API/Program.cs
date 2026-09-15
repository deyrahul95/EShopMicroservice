using Basket.Core.Domains;
using Basket.Core.Repositories;
using BuildingBlock.Behaviors;
using BuildingBlock.Exceptions.Handler;
using Carter;
using FluentValidation;
using Marten;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

var assembly = typeof(Program).Assembly;

// Add services to the container
builder.Services.AddOpenApi();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    var databaseConnection = builder.Configuration.GetConnectionString("Database")
        ?? throw new Exception("Database connection string can't be empty.");
    options.Connection(databaseConnection);
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

// Configured the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();

app.UseExceptionHandler(option => { });

app.Run();
