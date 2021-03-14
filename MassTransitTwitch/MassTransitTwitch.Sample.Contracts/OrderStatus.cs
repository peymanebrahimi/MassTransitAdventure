using System;

namespace MassTransitTwitch.Sample.Contracts
{
    public interface OrderStatus
    {
        Guid OrderId { get; }
        string State { get; }
    }
}