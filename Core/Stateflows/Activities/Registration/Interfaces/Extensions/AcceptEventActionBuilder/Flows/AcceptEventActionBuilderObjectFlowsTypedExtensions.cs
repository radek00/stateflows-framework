﻿using System.Diagnostics;
using Stateflows.Common.Extensions;
using Stateflows.Activities.Extensions;
using Stateflows.Activities.Registration.Interfaces;

namespace Stateflows.Activities.Typed
{
    public static class AcceptEventActionBuilderObjectFlowsTypedExtensions
    {
        [DebuggerHidden]
        public static IAcceptEventActionBuilder AddFlow<TToken, TTargetNode>(this IAcceptEventActionBuilder builder, ObjectFlowBuildAction<TToken> buildAction = null)
            where TTargetNode : class, IActivityNode
            => builder.AddFlow<TToken>(ActivityNode<TTargetNode>.Name, buildAction);

        [DebuggerHidden]
        public static IAcceptEventActionBuilder AddFlow<TToken, TFlow>(this IAcceptEventActionBuilder builder, string targetNodeName)
            where TFlow : class, IBaseFlow<TToken>
        {
            (builder as IInternal).Services.AddServiceType<TFlow>();

            return builder.AddFlow<TToken>(
                targetNodeName,
                b => b.AddObjectFlowEvents<TFlow, TToken>()
            );
        }

        [DebuggerHidden]
        public static IAcceptEventActionBuilder AddFlow<TToken, TFlow, TTargetNode>(this IAcceptEventActionBuilder builder)
            where TFlow : class, IBaseFlow<TToken>
            where TTargetNode : class, IActivityNode
            => builder.AddFlow<TToken, TFlow>(ActivityNode<TTargetNode>.Name);

        [DebuggerHidden]
        public static IAcceptEventActionBuilder AddFlow<TToken, TTransformedToken, TTransformationFlow>(this IAcceptEventActionBuilder builder, string targetNodeName)
            where TTransformationFlow : class, IFlowTransformation<TToken, TTransformedToken>
        {
            (builder as IInternal).Services.AddServiceType<TTransformationFlow>();

            return builder.AddFlow<TToken>(
                targetNodeName,
                b => b.AddObjectTransformationFlowEvents<TTransformationFlow, TToken, TTransformedToken>()
            );
        }

        [DebuggerHidden]
        public static IAcceptEventActionBuilder AddFlow<TToken, TTransformedToken, TTransformationFlow, TTargetNode>(this IAcceptEventActionBuilder builder)
            where TTransformationFlow : class, IFlowTransformation<TToken, TTransformedToken>
            where TTargetNode : class, IActivityNode
            => builder.AddFlow<TToken, TTransformedToken, TTransformationFlow>(ActivityNode<TTargetNode>.Name);
    }
}
