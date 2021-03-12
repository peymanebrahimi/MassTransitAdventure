using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransitKevinSmith.Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitKevinSmith.Shop
{
    public class ShopWork : IHostedService
    {
        private readonly IPublishEndpoint _publishEndpoint;
        readonly ILogger _logger;

        public ShopWork(ILoggerFactory loggerFactory, IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _logger = loggerFactory.CreateLogger<MassTransitConsoleHostedService>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {GetType().Name}");

            Console.WriteLine("Welcome to the Shop");
            Console.WriteLine("Press Q key to exit");
            Console.WriteLine("Press [0..9] key to order some products");
            Console.WriteLine(string.Join(Environment.NewLine, Products.
                Select((x, i) => $"[{i}]: {x.name} @ {x.price:C}")));

            var products = new List<(string name, decimal price, int Number)>();
            int number = 0;
            for (; ; )
            {
                var consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.Q)
                {
                    break;
                }

                if (char.IsNumber(consoleKeyInfo.KeyChar))
                {
                    // Hack: I don't care about ½ etc...
                    var product = Products[(int)char.GetNumericValue(consoleKeyInfo.KeyChar)];
                    products.Add(product);
                    Console.WriteLine($"Added {product.name}");
                }

                if (consoleKeyInfo.Key == ConsoleKey.Enter)
                {
                    await _publishEndpoint.Publish<IOrderRequested>(new
                    {
                        Products = products.Select(x => new {Name = x.name, Price = x.price, Number = ++number})
                            .ToList()
                    }, cancellationToken);

                    Console.WriteLine("Submitted Order");

                    products.Clear();
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {GetType().Name}");
            return Task.CompletedTask;
        }


        private static readonly IReadOnlyList<(string name, decimal price, int number)> Products = new List<(string, decimal, int)>
        {
            ("Bread", 1.20m,0),
            ("Milk", 0.50m,0),
            ("Rice", 1m,0),
            ("Pasta", 0.9m,0),
            ("Pasta", 0.9m,0),
            ("Cereals", 1.6m,0),
            ("Chocolate", 2m,0),
            ("Noodles", 1m,0),
            ("Pie", 1m,0),
            ("Sandwich", 1m,0),
        };
    }
}