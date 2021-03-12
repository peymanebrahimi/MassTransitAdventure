using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitTwitch.Contracts;
using Microsoft.Extensions.Logging;

namespace MassTransitTwitch.Components.Consumers
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            if (context.Message.CustomerNumber.Contains("test"))
            {
                await context.RespondAsync<OrderSubmissionRejected>(new
                {

                    OrderId = context.Message.OrderId,
                    Timestamp = InVar.Timestamp,
                    CustomerNumber = context.Message.CustomerNumber,
                    Reason = $"test customer cannot submit order {context.Message.CustomerNumber}"
                });
                return;
            }

            await context.RespondAsync<OrderSubmissionAccepted>(new
            {
                OrderId = context.Message.OrderId,
                Timestamp = InVar.Timestamp,
                CustomerNumber = context.Message.CustomerNumber,
            });
        }
    }
}