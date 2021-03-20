using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Twitch.Sample.Components.Consumers;
using Twitch.Sample.Components.StateMachines;

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
                    services.AddMassTransit(c =>
                    {
                        c.AddConsumersFromNamespaceContaining<SubmitOrderConsumer>();

                        c.AddSagaStateMachine<OrderStateMachine, OrderState>(typeof(OrderStateMachineDefinition))
                            .RedisRepository();

                        c.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h => { });
                            cfg.ConfigureEndpoints(context);
                            //cfg.ReceiveEndpoint("submit-order",
                            //    e =>
                            //    {
                            //        e.PrefetchCount = 16;
                            //        e.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(10)));
                            //        e.ConfigureConsumer<SubmitOrderConsumer>(context);
                            //    });
                        });
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
