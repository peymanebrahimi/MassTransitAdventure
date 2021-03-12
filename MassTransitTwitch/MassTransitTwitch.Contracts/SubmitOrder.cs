﻿using System;
using MassTransit;

namespace MassTransitTwitch.Contracts
{
    public interface SubmitOrder
    {
        Guid OrderId { get; }
        DateTime Timestamp { get; }

        string CustomerNumber { get; }
        string PaymentCardNumber { get; }

        //MessageData<string> Notes { get; }
    }
}
