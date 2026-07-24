using Carter;
using Marten;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});
builder.Services.AddMarten(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("Database")
        ?? throw new Exception("Database connection string can't be null!");
    options.Connection(connectionString);
}).UseLightweightSessions();

var app = builder.Build();

// Configured the HTTP request pipeline
app.MapCarter();

app.Run();
