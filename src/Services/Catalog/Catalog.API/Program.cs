using BuildingBlock.Behaviors;
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
});

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database")
        ?? throw new Exception("Database connection string can't be null!");
    options.Connection(connectionString);
}).UseLightweightSessions();

var app = builder.Build();

// Configured the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapCarter();

app.Run();
