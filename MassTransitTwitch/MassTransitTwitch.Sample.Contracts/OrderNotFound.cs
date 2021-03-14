using System;

namespace MassTransitTwitch.Sample.Contracts
{
    public interface OrderNotFound
    {
        Guid OrderId { get; }
    }
}