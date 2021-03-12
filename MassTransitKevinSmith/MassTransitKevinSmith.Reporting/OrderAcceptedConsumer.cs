using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MassTransitKevinSmith.Messages;

namespace MassTransitKevinSmith.Reporting
{
    public class OrderAcceptedConsumer : IConsumer<IOrderAccepted>
    {
        private readonly ReportStore _store;

        public OrderAcceptedConsumer(ReportStore store)
        {
            _store = store;
        }

        public Task Consume(ConsumeContext<IOrderAccepted> context)
        {
            _store.IncrementTotalOrdersAccepted(context.Message.Products.Sum(x => x.Price));
            _store.IncrementProductSales(context.Message.Products);

            return Task.CompletedTask;
        }
    }
}