using BuildingBlock.Behaviors;
using Carter;
using FluentValidation;
using Marten;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddOpenApi();

builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

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
