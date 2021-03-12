using System.Collections.Generic;

namespace MassTransitKevinSmith.Messages
{
    public interface IOrderRejected
    {
        IList<IProduct> Products { get; }
    }
}