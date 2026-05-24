using Ordering.API;
using Ordering.Application;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Data.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

//---------------------------------------------
//Infrastructure - EF Core
//Application - MediatR
//API - Carter, HealthChecks
//---------------------------------------------

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseApiServices();

if (app.Environment.IsDevelopment())
{
    var maxRetries = 5;
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    for (int i = 0; i < maxRetries; i++)
    {
        try
        {
            logger.LogInformation("Database initialization attempt {Attempt}/{MaxRetries}", i + 1, maxRetries);
            await app.InitializeDatabaseAsync();
            logger.LogInformation("Database initialized successfully");
            break;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Database initialization failed on attempt {Attempt}/{MaxRetries}. Error: {Message}",
                i + 1, maxRetries, ex.Message);

            if (i == maxRetries - 1)
            {
                logger.LogCritical("Database initialization failed after {MaxRetries} attempts", maxRetries);
                throw;
            }

            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}

app.Run();
