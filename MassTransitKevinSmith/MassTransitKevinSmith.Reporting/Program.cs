using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitKevinSmith.Reporting
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
                        c.AddConsumer<OrderRequestedConsumer>();
                        c.AddConsumer<OrderAcceptedConsumer>();
                        c.AddConsumer<OrderRejectedConsumer>();

                        c.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h => { });

                            cfg.ReceiveEndpoint("Reporting",
                                e =>
                                {
                                    e.PrefetchCount = 16;
                                    e.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(10)));
                                    e.ConfigureConsumer<OrderRequestedConsumer>(context);
                                    e.ConfigureConsumer<OrderAcceptedConsumer>(context);
                                    e.ConfigureConsumer<OrderRejectedConsumer>(context);
                                });
                        });
                    });
                    services.AddHostedService<MassTransitConsoleHostedService>();
                    services.AddHostedService<ReportingWork>();
                    services.AddSingleton<ReportStore>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                }).RunConsoleAsync();
        }
    }
}
