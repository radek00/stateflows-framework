﻿using System.Threading.Tasks;
using Stateflows.Common.Interfaces;
using Stateflows.StateMachines;
using Stateflows.StateMachines.Events;

namespace Stateflows.Common.StateMachines.Classes
{
    internal class StateMachineWrapper : IStateMachine
    {
        BehaviorId IBehavior.Id => Behavior.Id;

        private IBehavior Behavior { get; }

        public StateMachineWrapper(IBehavior consumer)
        {
            Behavior = consumer;
        }

        public Task<RequestResult<CurrentStateResponse>> GetCurrentStateAsync()
            => RequestAsync(new CurrentStateRequest());

        public Task<SendResult> SendAsync<TEvent>(TEvent @event)
            where TEvent : Event, new()
            => Behavior.SendAsync(@event);

        public Task<RequestResult<TResponse>> RequestAsync<TResponse>(Request<TResponse> request)
            where TResponse : Response, new()
            => Behavior.RequestAsync(request);
    }
}
