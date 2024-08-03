﻿using System.Diagnostics;
using Stateflows.StateMachines.Events;
using Stateflows.StateMachines.Registration.Interfaces;

namespace Stateflows.StateMachines.Typed
{
    public static class TypedFinalizedCompositeStateBuilderElseDefaultTransitionTypedExtensions
    {
        [DebuggerHidden]
        public static ITypedFinalizedCompositeStateBuilder AddElseDefaultTransition<TElseTransition, TTargetState>(this ITypedFinalizedCompositeStateBuilder builder)
            where TElseTransition : class, IDefaultTransitionEffect
            where TTargetState : class, IVertex
            => builder.AddElseDefaultTransition<TElseTransition>(State<TTargetState>.Name);

        [DebuggerHidden]
        public static ITypedFinalizedCompositeStateBuilder AddElseDefaultTransition<TElseTransition>(this ITypedFinalizedCompositeStateBuilder builder, string targetStateName)
            where TElseTransition : class, IDefaultTransitionEffect
            => (builder as IStateBuilder).AddElseDefaultTransition<TElseTransition>(targetStateName) as ITypedFinalizedCompositeStateBuilder;

        [DebuggerHidden]
        public static ITypedFinalizedCompositeStateBuilder AddElseDefaultTransition<TTargetState>(this ITypedFinalizedCompositeStateBuilder builder, ElseDefaultTransitionBuildAction transitionBuildAction = null)
            where TTargetState : class, IVertex
            => builder.AddElseDefaultTransition(State<TTargetState>.Name, transitionBuildAction);
    }
}
