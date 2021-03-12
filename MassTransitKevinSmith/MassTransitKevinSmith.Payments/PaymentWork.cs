using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitKevinSmith.Payments
{
    public class PaymentWork : IHostedService
    {
        readonly ILogger _logger;
        public PaymentWork(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<MassTransitConsoleHostedService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {GetType().Name}");
            Console.WriteLine("Welcome to Payments");
            Console.WriteLine("Press Q key to exit");
            while (Console.ReadKey(true).Key != ConsoleKey.Q) ;
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {GetType().Name}");
            return Task.CompletedTask;
        }
    }
}