using Discount.Grpc.Data;
using Discount.Grpc.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Override URLs to bind to all interfaces when in Docker
if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")))
{
    builder.WebHost.UseUrls("https://+:8081", "http://+:8080");
}

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddDbContext<DiscountContext>(
    opt =>
    {
        opt.UseSqlite(builder.Configuration.GetConnectionString("Database"));
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMigration(); // adding extention at the start of pipeline to ensure that database is created.
app.MapGrpcService<DiscountService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
