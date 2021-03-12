using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitKevinSmith.Messages;

namespace MassTransitKevinSmith.Payments
{
    public class OrderRequestedConsumer : IConsumer<IOrderRequested>
    {
        private bool TakePayment(IOrderRequested contextMessage)
        {
            // 3 in 10 payments fail
            var failsCondition = new[] { 1, 5, 6 };
            var paymentFailed = failsCondition.Contains(Random.Next(1, 10));

            if (paymentFailed) Console.WriteLine("TakePayment: Payment Failed: " + contextMessage.Products[0].Number);
            if (!paymentFailed) Console.WriteLine("TakePayment: Payment Successful: " + contextMessage.Products[0].Number);

            return !paymentFailed;
        }

        private static readonly Random Random = new Random();

        public async Task Consume(ConsumeContext<IOrderRequested> context)
        {
            if (TakePayment(context.Message))
            {
                await context.Publish<IOrderAccepted>(new { context.Message.Products });
            }
            else
            {
                await context.Publish<IOrderRejected>(new { context.Message.Products });
                //throw new Exception("Payment Failed Exception: " + context.Message.Products[0].Number);
            }
        }
    }
}