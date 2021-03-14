using System;

namespace MassTransitTwitch.Sample.Contracts
{
    public interface CheckOrder
    {
        Guid OrderId { get; }
    }
}
