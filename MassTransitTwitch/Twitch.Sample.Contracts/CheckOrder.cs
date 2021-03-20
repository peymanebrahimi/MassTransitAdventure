using System;

namespace Twitch.Sample.Contracts
{
    public interface CheckOrder
    {
        Guid OrderId { get; }
    }
}
