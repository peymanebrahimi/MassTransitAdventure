using System;
using System.Threading.Tasks;
using MassTransit;
using SharedModels;

namespace InventoryService
{
    public class OrderConsumer : IConsumer<Order>
    {
        public async Task Consume(ConsumeContext<Order> context)
        {
            await Console.Out.WriteLineAsync($"{context.Message.Name} consumed.");
        }
    }
}