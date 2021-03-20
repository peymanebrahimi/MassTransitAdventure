using System;

namespace Twitch.Sample.Contracts
{
    public interface OrderStatus
    {
        Guid OrderId { get; }
        string State { get; }
    }
}