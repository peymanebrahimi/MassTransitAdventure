using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitKevinSmith.Shop
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var hostBuilder =
            await Host
                //.ConfigureAppConfiguration((hostingContext, config) =>
                //{
                //    config.AddJsonFile("appsettings.json", optional: true);
                //    config.AddEnvironmentVariables();

                //    if (args != null)
                //        config.AddCommandLine(args);
                //})
                .CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    services.AddMassTransit(c =>
                    {
                        c.AddConsumer<OrderRequestedFaultConsumer>();

                        c.UsingRabbitMq((context, cfg) =>
                        {
                            cfg.Host("localhost", "/", h => {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ReceiveEndpoint("Shop",
                                e =>
                                {
                                    e.PrefetchCount = 16;
                                    e.UseMessageRetry(r => r.Interval(2, TimeSpan.FromSeconds(10)));
                                    e.ConfigureConsumer<OrderRequestedFaultConsumer>(context);
                                });
                        });
                    });
                    //services.AddHostedService<MassTransitConsoleHostedService>();
                    //services.AddSingleton<IPublishEndpoint>();
                    services.AddHostedService<ShopWork>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole();
                }).RunConsoleAsync();


            //var host = hostBuilder.Build();
            //using var scope = host.Services.CreateScope();
            //var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            //Console.WriteLine("Welcome to the Shop");
            //Console.WriteLine("Press Q key to exit");
            //Console.WriteLine("Press [0..9] key to order some products");
            //Console.WriteLine(string.Join(Environment.NewLine, Products.
            //    Select((x, i) => $"[{i}]: {x.name} @ {x.price:C}")));

            //var products = new List<(string name, decimal price)>();

            //for (; ; )
            //{
            //    var consoleKeyInfo = Console.ReadKey(true);
            //    if (consoleKeyInfo.Key == ConsoleKey.Q)
            //    {
            //        break;
            //    }

            //    if (char.IsNumber(consoleKeyInfo.KeyChar))
            //    {
            //        // Hack: I don't care about ½ etc...
            //        var product = Products[(int)char.GetNumericValue(consoleKeyInfo.KeyChar)];
            //        products.Add(product);
            //        Console.WriteLine($"Added {product.name}");
            //    }

            //    if (consoleKeyInfo.Key == ConsoleKey.Enter)
            //    {
            //        await publishEndpoint.Publish<IOrderRequested>(new
            //        {
            //            Products = products.Select(x => new { Name = x.name, Price = x.price }).ToList()
            //        });

            //        Console.WriteLine("Submitted Order");

            //        products.Clear();
            //    }
            //}

            //await host.RunConsoleAsync();
        }


    }
}
