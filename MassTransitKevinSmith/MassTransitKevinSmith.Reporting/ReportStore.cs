using System.Collections.Concurrent;
using System.Collections.Generic;
using MassTransitKevinSmith.Messages;

namespace MassTransitKevinSmith.Reporting
{
    public class ReportStore
    {
        private decimal _totalOrdersRequested;
        private decimal _totalOrdersAccepted;
        private decimal _totalOrdersRejected;
        private readonly ConcurrentDictionary<string, decimal> _productSales = new ConcurrentDictionary<string, decimal>();

        private readonly object _totalOrdersRequestedLock = new object();
        private readonly object _totalOrdersAcceptedLock = new object();
        private readonly object _totalOrdersRejectedLock = new object();


        public ReportStore()
        {
            _totalOrdersRequested = 0;
            _totalOrdersAccepted = 0;
            _totalOrdersRejected = 0;
        }

        public decimal TotalOrdersRequested => _totalOrdersRequested;

        public decimal TotalOrdersAccepted => _totalOrdersAccepted;
        public decimal TotalOrdersRejected => _totalOrdersRejected;

        public IReadOnlyDictionary<string, decimal> ProductSales => _productSales;

        public void IncrementTotalOrdersRequested(decimal orderTotal)
        {
            lock (_totalOrdersRequestedLock)
            {
                _totalOrdersRequested += orderTotal;
            }
        }

        public void DecrementTotalOrdersRequested(decimal orderTotal)
        {
            lock (_totalOrdersRequestedLock)
            {
                _totalOrdersRequested -= orderTotal;
            }
            lock (_totalOrdersRejectedLock)
            {
                _totalOrdersRejected += orderTotal;
            }
        }

        public void IncrementTotalOrdersAccepted(decimal orderTotal)
        {
            lock (_totalOrdersAcceptedLock)
            {
                _totalOrdersAccepted += orderTotal;
            }
        }

        public void IncrementProductSales(IEnumerable<IProduct> products)
        {
            foreach (var product in products)
            {
                _productSales.AddOrUpdate(product.Name, product.Price, (s, arg2) => arg2 += product.Price);
            }
        }
    }
}