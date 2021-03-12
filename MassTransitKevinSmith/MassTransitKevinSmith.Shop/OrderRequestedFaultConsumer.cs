using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitKevinSmith.Messages;

namespace MassTransitKevinSmith.Shop
{
    public class OrderRequestedFaultConsumer : IConsumer<Fault<IOrderRequested>>, IConsumer<IOrderRejected>
    {
        public Task Consume(ConsumeContext<Fault<IOrderRequested>> context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The following order was not processed:");

            foreach (var productGroup in context.Message.Message.Products.GroupBy(x => x.Name))
            {
                Console.WriteLine($"{productGroup.Key} x{productGroup.Count()}");
            }
            Console.ResetColor();
            return Task.CompletedTask;
        }

        public Task Consume(ConsumeContext<IOrderRejected> context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The following order was rejected:");

            foreach (var productGroup in context.Message.Products.GroupBy(x => x.Name))
            {
                Console.WriteLine($"{productGroup.Key} x{productGroup.Count()}");
            }
            Console.ResetColor();
            return Task.CompletedTask;
        }
    }
}