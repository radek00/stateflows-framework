﻿using System.Net.Mime;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Stateflows.Common;
using Stateflows.Common.Utilities;
using Stateflows.Common.Interfaces;
using Stateflows.Common.Extensions;
using Stateflows.Common.Transport.Classes;
using Stateflows.Common.Registration.Interfaces;
using Stateflows.StateMachines.Interceptors;

namespace Stateflows.Transport.Http
{
    public static class DependencyInjection
    {
        private static bool apiMapped = false;

        [DebuggerHidden]
        public static IEndpointRouteBuilder MapStateflowsHttpTransport(this IEndpointRouteBuilder builder, Action<RouteHandlerBuilder>? routeHandlerBuilderAction = null)
        {
            if (!apiMapped)
            {
                var routeBuilder = builder.MapPost(
                    "/stateflows/send",
                    async (
                        HttpContext context,
                        IBehaviorLocator locator,
                        INotificationsHub hub
                    ) =>
                    {
                        var responseTime = DateTime.Now;
                        using var reader = new StreamReader(context.Request.Body);
                        var body = await reader.ReadToEndAsync();
                        var input = StateflowsJsonConverter.DeserializeObject<StateflowsRequest>(body);

                        var behaviorId = new BehaviorId(input.BehaviorId.Type, input.BehaviorId.Name, input.BehaviorId.Instance);
                        if (locator.TryLocateBehavior(behaviorId, out var behavior))
                        {
                            var result = await behavior.SendAsync(input.Event);
                            return Results.Text(
                                StateflowsJsonConverter.SerializePolymorphicObject(
                                    new StateflowsResponse()
                                    {
                                        EventStatus = result.Status,
                                        Validation = result.Validation,
                                        Response = result.Status == EventStatus.Consumed
                                            ? result.Event.GetResponse()
                                            : null,
                                        Notifications = result.Status != EventStatus.Rejected
                                            ? hub.Notifications.GetPendingNotifications(behaviorId, input.Watches)
                                            : Array.Empty<Notification>(),
                                        ResponseTime = responseTime,
                                    },
                                    true
                                ),
                                MediaTypeNames.Application.Json
                            );
                        }
                        else
                        {
                            return Results.NotFound();
                        }
                    }
                );

                routeHandlerBuilderAction?.Invoke(routeBuilder);

                routeBuilder = builder.MapGet(
                    "/stateflows/availableClasses",
                    (IBehaviorClassesProvider provider) => Results.Ok(provider.AllBehaviorClasses)
                );

                routeHandlerBuilderAction?.Invoke(routeBuilder);

                apiMapped = true;
            }

            return builder;
        }

        public static IStateflowsBuilder AddAspAuthorization(this IStateflowsBuilder stateflowsBuilder)
        {
            stateflowsBuilder.AddInterceptor<AuthorizationInterceptor>();
            return stateflowsBuilder;
        }
    }
}
