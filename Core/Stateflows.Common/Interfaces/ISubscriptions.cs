﻿using System.Threading.Tasks;

namespace Stateflows.Common
{
    public interface ISubscriptions
    {
        /// <summary>
        /// Subscribes for notifications from given behavior (by sending <see cref="SubscriptionRequest"/> to it).<br/>
        /// Subscription is durable over time; the only way to end it is by calling <see cref="UnsubscribeAsync"/>.
        /// </summary>
        /// <typeparam name="TNotification">Subscribed notification type</typeparam>
        /// <param name="behaviorId">Identifier of a behavior being subscribed to</param>
        /// <returns>Task providing <see cref="SubscriptionResponse"/></returns>
        Task<RequestResult<SubscriptionResponse>> SubscribeAsync<TNotification>(BehaviorId behaviorId)
            where TNotification : Notification, new();

        /// <summary>
        /// Unsubscribes for notifications from given behavior (by sending <see cref="UnsubscriptionRequest"/> to it).
        /// </summary>
        /// <typeparam name="TNotification">Unsubscribed notification type</typeparam>
        /// <param name="behaviorId">Identifier of a behavior being unsubscribed to</param>
        /// <returns>Task providing <see cref="UnsubscriptionResponse"/></returns>
        Task<RequestResult<UnsubscriptionResponse>> UnsubscribeAsync<TNotification>(BehaviorId behaviorId)
            where TNotification : Notification, new();
    }
}