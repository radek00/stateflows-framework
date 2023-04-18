﻿using Stateflows.Common;
using Stateflows.StateMachines.Registration;
using Stateflows.StateMachines.Registration.Interfaces;

namespace Stateflows.StateMachines
{
    public static class StateTransitionsBuilderInternalTransitionTypedExtensions
    {
        public static IStateTransitionsBuilder AddInternalTransition<TEvent, TTransition>(this IStateTransitionsBuilder builder)
            where TEvent : Event, new()
            where TTransition : Transition<TEvent>
            => builder.AddTransition<TEvent, TTransition>(Constants.DefaultTransitionTarget);
    }
}
