﻿using System;

namespace MassTransitTwitch.Sample.Contracts
{
    public interface CustomerAccountClosed
    {
        Guid CustomerId { get; }
        string CustomerNumber { get; }
    }
}