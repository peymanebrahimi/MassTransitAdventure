using System;

namespace Twitch.Sample.Contracts
{
    public interface FulfillOrder
    {
        Guid OrderId { get; }

        string CustomerNumber { get; }
        string PaymentCardNumber { get; }
    }
}