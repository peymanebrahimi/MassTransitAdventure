﻿using System;
using MassTransit;

namespace Twitch.Sample.Components.Consumers
{
    public class FulfillOrderConsumerDefinition :
        ConsumerDefinition<FulfillOrderConsumer>
    {
        public FulfillOrderConsumerDefinition()
        {
            ConcurrentMessageLimit = 20;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<FulfillOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r =>
            {
                r.Ignore<InvalidOperationException>();

                r.Interval(3, 1000);
            });

            endpointConfigurator.DiscardFaultedMessages();
        }
    }
}