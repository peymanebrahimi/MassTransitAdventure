using System;
using Automatonymous;
using GreenPipes;
using MassTransit;
using MassTransit.Definition;
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
                    .TransitionTo(Submitted)
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

    public class OrderStateMachineDefinition : SagaDefinition<OrderState>
    {
        public OrderStateMachineDefinition()
        {
            ConcurrentMessageLimit = 4;
        }
        protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<OrderState> sagaConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 5000, 10000));
            endpointConfigurator.UseInMemoryOutbox();
        }
    }
}