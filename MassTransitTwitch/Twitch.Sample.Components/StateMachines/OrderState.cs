﻿using System;
using MassTransit;

namespace Twitch.Sample.Components.StateMachines
{
    public class OrderState : SagaStateMachineInstance, ISagaVersion
    {
        public Guid CorrelationId { get; set; }
        public int Version { get; set; }
        public byte[] RowVersion { get; set; }
        
        public string CurrentState { get; set; }

        public string CustomerNumber { get; set; }
        //public string PaymentCardNumber { get; set; }

        //public string FaultReason { get; set; }

        public DateTime? SubmitDate { get; set; }
        public DateTime? Updated { get; set; }
    }

    
}