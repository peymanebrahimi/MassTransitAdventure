using System;
using System.Threading.Tasks;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitch.Sample.Components.Consumers;
using Twitch.Sample.Components.CourierActivities;
using Twitch.Sample.Components.StateMachines;
using Twitch.Sample.Components.StateMachines.OrderStateMachineActivities;
using Twitch.Warehouse.Contracts;

namespace Twitch.Sample.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await Host
                .CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    //services.AddScoped<AcceptOrderActivity>();

                    services.AddMassTransit(serviceCollectionBusConfigurator =>
                        {
                            serviceCollectionBusConfigurator.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();

                            //serviceCollectionBusConfigurator.AddActivitiesFromNamespaceContaining<AllocateInventoryActivity>();

                            serviceCollectionBusConfigurator
                                .AddSagaStateMachine<OrderStateMachine, OrderState>(typeof(OrderStateMachineDefinition))
                                .RedisRepository();

                            serviceCollectionBusConfigurator.UsingRabbitMq((busRegistrationContext, busFactoryConfigurator) =>
                            {
                                busFactoryConfigurator.Host("rabbitmq", "/", h =>
                                {
                                    h.Username("guest");
                                    h.Password("guest");
                                });
                                
                                busFactoryConfigurator.ReceiveEndpoint(
                                    e =>
                                    {
                                        e.PrefetchCount = 25;
                                        e.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(10)));
                                        e.ConfigureConsumer<SubmitOrderConsumer>(busRegistrationContext);
                                    }
                                );

                                busFactoryConfigurator.ConfigureEndpoints(busRegistrationContext);
                            });

                            serviceCollectionBusConfigurator.AddRequestClient<AllocateInventory>();
                        });

                    services.AddHostedService<MassTransitConsoleHostedService>();

                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                }).RunConsoleAsync();
        }
    }


}
