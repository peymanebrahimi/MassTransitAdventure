﻿using GreenPipes;
using MassTransit;
using MassTransit.Definition;

namespace Twitch.Warehouse.Components.StateMachines
{
    public class AllocateStateMachineDefinition :
        SagaDefinition<AllocationState>
    {
        public AllocateStateMachineDefinition()
        {
            ConcurrentMessageLimit = 10;
        }

        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator,
            ISagaConfigurator<AllocationState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}