﻿using System.Diagnostics;
using Stateflows.StateMachines.Extensions;
using Stateflows.StateMachines.Registration.Interfaces;
using Stateflows.StateMachines.Registration.Interfaces.Internal;

namespace Stateflows.StateMachines.Typed
{
    public static class TypedInitializedCompositeStateBuilderTyped2Extensions
    {
        #region AddFinalState
        [DebuggerHidden]
        public static ITypedFinalizedCompositeStateBuilder AddState<TFinalState>(this ITypedInitializedCompositeStateBuilder builder, string stateName = FinalState.Name)
            where TFinalState : class, IFinalState
            => builder.AddFinalState(stateName);
        #endregion

        #region AddState
        [DebuggerHidden]
        public static ITypedInitializedCompositeStateBuilder AddState<TState>(this ITypedInitializedCompositeStateBuilder builder, StateTransitionsBuildAction stateBuildAction = null)
            where TState : class, IBaseState
            => builder.AddState<TState>(State<TState>.Name, stateBuildAction);

        [DebuggerHidden]
        public static ITypedInitializedCompositeStateBuilder AddState<TState>(this ITypedInitializedCompositeStateBuilder builder, string stateName, StateTransitionsBuildAction stateBuildAction = null)
            where TState : class, IBaseState
        {
            (builder as IInternal).Services.RegisterState2<TState>();

            return builder.AddState(
                stateName,
                b =>
                {
                    b.AddStateEvents2<TState, IStateBuilder>();

                    stateBuildAction?.Invoke(b as ITypedStateBuilder);
                }
            );
        }
        #endregion

        #region AddCompositeState
        [DebuggerHidden]
        public static ITypedInitializedCompositeStateBuilder AddCompositeState<TCompositeState>(this ITypedInitializedCompositeStateBuilder builder, CompositeStateTransitionsBuildAction compositeStateBuildAction)
            where TCompositeState : class, IBaseCompositeState
            => builder.AddCompositeState<TCompositeState>(State<TCompositeState>.Name, compositeStateBuildAction);

        [DebuggerHidden]
        public static ITypedInitializedCompositeStateBuilder AddCompositeState<TCompositeState>(this ITypedInitializedCompositeStateBuilder builder, string stateName, CompositeStateTransitionsBuildAction compositeStateBuildAction)
            where TCompositeState : class, IBaseCompositeState
        {
            (builder as IInternal).Services.RegisterState2<TCompositeState>();

            return builder.AddCompositeState(
                stateName,
                b =>
                {
                    (b as IInitializedCompositeStateBuilder).AddStateEvents2<TCompositeState, IInitializedCompositeStateBuilder>();
                    (b as IInitializedCompositeStateBuilder).AddCompositeStateEvents2<TCompositeState, IInitializedCompositeStateBuilder>();

                    compositeStateBuildAction?.Invoke(b as ITypedCompositeStateBuilder);
                }
            );
        }
        #endregion
    }
}
