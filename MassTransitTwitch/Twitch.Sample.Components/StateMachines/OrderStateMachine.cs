using System;
using MassTransit;
using Twitch.Sample.Components.StateMachines.OrderStateMachineActivities;
using Twitch.Sample.Contracts;

namespace Twitch.Sample.Components.StateMachines
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));

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

            Event(() => OrderAccepted, x => x.CorrelateById(m => m.Message.OrderId));

            Event(() => AccountClosed, x => x.CorrelateBy((saga, context) => saga.CustomerNumber == context.Message.CustomerNumber));

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
                Ignore(OrderSubmitted),
                When(AccountClosed)
                    .TransitionTo(Canceled),
                When(OrderAccepted)
                    .Activity(x => x.OfType<AcceptOrderActivity>())
                    .TransitionTo(Accepted));

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
        public State Canceled { get; private set; }
        public State Accepted { get; private set; }
        
        public Event<OrderSubmitted> OrderSubmitted { get; private set; }
        public Event<CheckOrder> OrderStatusRequested { get; private set; }
        public Event<OrderAccepted> OrderAccepted { get; private set; }
        public Event<CustomerAccountClosed> AccountClosed { get; private set; }
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