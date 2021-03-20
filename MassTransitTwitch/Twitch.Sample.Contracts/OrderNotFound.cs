using System;

namespace Twitch.Sample.Contracts
{
    public interface OrderNotFound
    {
        Guid OrderId { get; }
    }
}