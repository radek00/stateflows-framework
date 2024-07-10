﻿using Stateflows.Common;
using Stateflows.StateMachines.Extensions;
using Stateflows.StateMachines.Registration.Interfaces;
using Stateflows.StateMachines.Registration.Interfaces.Internal;

namespace Stateflows.StateMachines.Typed
{
    public static class FinalizedCompositeStateBuilderTransitionTypedExtensions
    {
        public static IFinalizedCompositeStateBuilder AddTransition<TEvent, TTransition, TTargetState>(this IFinalizedCompositeStateBuilder builder)
            where TEvent : Event, new()
            where TTransition : class, IBaseTransition<TEvent>
            where TTargetState : class, IBaseState
            => AddTransition<TEvent, TTransition>(builder, State<TTargetState>.Name);

        public static IFinalizedCompositeStateBuilder AddTransition<TEvent, TTransition>(this IFinalizedCompositeStateBuilder builder, string targetVertexName)
            where TEvent : Event, new()
            where TTransition : class, IBaseTransition<TEvent>
        {
            (builder as IInternal).Services.RegisterTransition2<TTransition, TEvent>();

            return builder.AddTransition<TEvent>(
                targetVertexName,
                t => t.AddTransitionEvents2<TTransition, TEvent>()
            );
        }

        public static IFinalizedCompositeStateBuilder AddTransition<TEvent, TTargetState>(this IFinalizedCompositeStateBuilder builder, TransitionBuildAction<TEvent> transitionBuildAction = null)
            where TEvent : Event, new()
            where TTargetState : class, IBaseState
            => builder.AddTransition(State<TTargetState>.Name, transitionBuildAction);
    }
}
