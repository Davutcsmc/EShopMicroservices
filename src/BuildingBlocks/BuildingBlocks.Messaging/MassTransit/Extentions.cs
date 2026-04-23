using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit
{
    public static class Extentions
    {
        public static IServiceCollection AddMessageBroker
            (this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
        {
            // Add MassTransit configuration here
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter(); // Optional: Use kebab-case for endpoint names

                // Add consumers from the specified assembly
                if (assembly != null)
                {
                    config.AddConsumers(assembly);
                }
                // Configure the message broker (e.g., RabbitMQ, Azure Service Bus, etc.)
                config.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                    {
                        host.Username(configuration["MessageBroker:Username"]);
                        host.Password(configuration["MessageBroker:Password"]);
                    });
                    // Configure endpoints, etc.
                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
