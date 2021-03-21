﻿using System;
using Automatonymous;
using MassTransit.Saga;
using MongoDB.Bson.Serialization.Attributes;

namespace Twitch.Warehouse.Components.StateMachines
{
    public class AllocationState :
        SagaStateMachineInstance,
        ISagaVersion
    {
        public string CurrentState { get; set; }

        public Guid? HoldDurationToken { get; set; }

        public int Version { get; set; }

        [BsonId]
        public Guid CorrelationId { get; set; }
    }
}
