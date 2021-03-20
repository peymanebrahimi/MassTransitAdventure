using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;
using MassTransitTwitch.Sample.Contracts;

namespace MassTransitTwitch.Sample.Components.StateMachines.OrderStateMachineActivities
{
    public class AcceptOrderActivity : Activity<OrderState, OrderAccepted>
    {
        public void Probe(ProbeContext context)
        {
            context.CreateScope("order-accepted");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, OrderAccepted> context, Behavior<OrderState, OrderAccepted> next)
        {
            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, OrderAccepted, TException> context,
            Behavior<OrderState, OrderAccepted> next) where TException : Exception
        {
            return next.Faulted(context);
        }
    }
}