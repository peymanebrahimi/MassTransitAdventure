using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitKevinSmith.Messages;

namespace MassTransitKevinSmith.Reporting
{
    public class OrderRejectedConsumer : IConsumer<IOrderRejected>
    {
        private readonly ReportStore _store;

        public OrderRejectedConsumer(ReportStore store)
        {
            _store = store;
        }

        public Task Consume(ConsumeContext<IOrderRejected> context)
        {
            _store.DecrementTotalOrdersRequested(context.Message.Products.Sum(x => x.Price));

            return Task.CompletedTask;
        }
    }
}