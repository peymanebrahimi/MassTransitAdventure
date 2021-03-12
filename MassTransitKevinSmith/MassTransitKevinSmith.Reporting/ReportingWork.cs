using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MassTransitKevinSmith.Reporting
{
    public class ReportingWork : IHostedService
    {
        private readonly ReportStore _reportStore;
        readonly ILogger _logger;

        public ReportingWork(ILoggerFactory loggerFactory, ReportStore reportStore)
        {
            _reportStore = reportStore;
            _logger = loggerFactory.CreateLogger<MassTransitConsoleHostedService>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting {GetType().Name}");
            for (; ; )
            {
                var consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.Key == ConsoleKey.Q) break;
                if (consoleKeyInfo.Key == ConsoleKey.R)
                {
                    Console.WriteLine("-- Product Sales --");
                    Console.WriteLine(string.Join(Environment.NewLine,
                        _reportStore.ProductSales.Select(x => $"{x.Key}: {x.Value:C}")));

                    Console.WriteLine("-- Totals --");
                    Console.WriteLine($"TotalOrdersRequested: {_reportStore.TotalOrdersRequested:C}");
                    Console.WriteLine($"TotalOrdersRequested: {_reportStore.TotalOrdersRequested:C}");
                    Console.WriteLine($"TotalOrdersRejected: {_reportStore.TotalOrdersRejected:C}");
                }
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping {GetType().Name}");
            return Task.CompletedTask;
        }
    }
}