using System.Collections.Generic;

namespace MassTransitKevinSmith.Messages
{
    public interface IOrderAccepted
    {
        IList<IProduct> Products { get; }
    }
}