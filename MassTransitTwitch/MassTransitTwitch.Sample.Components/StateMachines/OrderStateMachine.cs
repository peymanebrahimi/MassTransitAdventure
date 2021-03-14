using System;
using Automatonymous;
using MassTransit;
using MassTransitTwitch.Sample.Contracts;

namespace MassTransitTwitch.Sample.Components.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderSubmitted,
                x => x
                    .CorrelateById(m => m.Message.OrderId));

            Event(() => OrderStatusRequested,
                    (IEventCorrelationConfigurator<OrderState, CheckOrder> x) =>
                    {
                        x.CorrelateById(m => m.Message.OrderId);
                        x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                        {
                            if (context.RequestId.HasValue)
                            {
                                await context.RespondAsync<OrderNotFound>(new { context.Message.OrderId });
                            }
                        }));
                    }
                );

            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.CustomerNumber = context.Data.CustomerNumber;
                        context.Instance.SubmitDate = context.Data.Timestamp;
                        context.Instance.Updated = DateTime.UtcNow;
                    })
                );

            During(Submitted,
                Ignore(OrderSubmitted));

            DuringAny(
                When(OrderStatusRequested)
                    .RespondAsync(x => x.Init<OrderStatus>(new
                    {
                        OrderId = x.Instance.CorrelationId,
                        State = x.Instance.CurrentState
                    })));

            DuringAny(
                When(OrderSubmitted)
                    .Then(x =>
                    {
                        x.Instance.SubmitDate ??= x.Data.Timestamp;
                        x.Instance.CustomerNumber ??= x.Data.CustomerNumber;
                    }));
        }

        public State Submitted { get; private set; }
        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<CheckOrder> OrderStatusRequested { get; private set; }
    }
}