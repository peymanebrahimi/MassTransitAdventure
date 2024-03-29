﻿using System;
using System.Threading.Tasks;
using MassTransit;
using Twitch.Sample.Contracts;

namespace Twitch.Sample.Components.StateMachines.OrderStateMachineActivities
{
    public class AcceptOrderActivity : IStateMachineActivity<OrderState, OrderAccepted>
    {
        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-accepted");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, OrderAccepted> context, 
            IBehavior<OrderState, OrderAccepted> next)
        {
            Console.WriteLine($"Hello, World. Order is {context.Data.OrderId}");

            var consumeContext = context.GetPayload<ConsumeContext>();

            var sendEndpoint = await consumeContext.GetSendEndpoint(new Uri("queue:fulfill-order")); // FulfillOrder

            await sendEndpoint.Send<FulfillOrder>(new
            {
                context.Data.OrderId,
                context.Instance.CustomerNumber,
                //context.Instance.PaymentCardNumber,
            });

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderAccepted, TException> context, IBehavior<OrderState, OrderAccepted> next) where TException : Exception
        {
            throw new NotImplementedException();
        }
    }
}