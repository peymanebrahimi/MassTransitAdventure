using System.Collections.Generic;

namespace MassTransitKevinSmith.Messages
{
    public interface IOrderRequested
    {
        IList<IProduct> Products { get; }
    }
}
