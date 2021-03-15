using System.Threading.Tasks;
using MassTransit;
using MassTransitTwitch.Sample.Contracts;
using Microsoft.Extensions.Logging;

namespace MassTransitTwitch.Sample.Components.Consumers
{
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer()
        {
        }
        
        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            if (context.Message.CustomerNumber.Contains("TEST"))
            {
                if (context.RequestId != null)
                    await context.RespondAsync<OrderSubmissionRejected>(new
                    {
                        InVar.Timestamp,
                        context.Message.OrderId,
                        context.Message.CustomerNumber,
                        Reason = $"Test Customer cannot submit orders: {context.Message.CustomerNumber}"
                    });

                return;
            }

            await context.Publish<OrderSubmitted>(new
            {

                OrderId = context.Message.OrderId,
                Timestamp = context.Message.Timestamp,
                CustomerNumber = context.Message.CustomerNumber
            });

            if (context.RequestId != null)
                await context.RespondAsync<OrderSubmissionAccepted>(new
                {
                    OrderId = context.Message.OrderId,
                    Timestamp = InVar.Timestamp,
                    CustomerNumber = context.Message.CustomerNumber,
                });
        }
    }
}