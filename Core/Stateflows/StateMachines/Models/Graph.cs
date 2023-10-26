﻿using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Stateflows.StateMachines.Interfaces;
using Stateflows.StateMachines.Registration;
using Stateflows.StateMachines.Registration.Interfaces;
using Stateflows.Common.Models;
using Stateflows.StateMachines.Exceptions;

namespace Stateflows.StateMachines.Models
{
    internal class Graph
    {
        public Dictionary<string, int> InitCounter = new Dictionary<string, int>();

        public Graph(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public Type StateMachineType { get; set; }
        public string InitialVertexName { get; set; }
        public Vertex InitialVertex { get; set; }
        public Dictionary<string, Vertex> Vertices { get; set; } = new Dictionary<string, Vertex>();
        public Dictionary<string, Vertex> AllVertices { get; set; } = new Dictionary<string, Vertex>();
        public List<Edge> AllEdges { get; set; } = new List<Edge>();

        private Dictionary<string, Logic<StateMachineActionAsync>> initializers = null;
        public Dictionary<string, Logic<StateMachineActionAsync>> Initializers
            => initializers ??= new Dictionary<string, Logic<StateMachineActionAsync>>();

        private Logic<StateMachineActionAsync> finalize = null;
        public Logic<StateMachineActionAsync> Finalize
            => finalize ??= new Logic<StateMachineActionAsync>()
                {
                    Name = Constants.Finalize
                };

        public List<StateMachineExceptionHandlerFactory> ExceptionHandlerFactories { get; set; } = new List<StateMachineExceptionHandlerFactory>();

        public List<StateMachineInterceptorFactory> InterceptorFactories { get; set; } = new List<StateMachineInterceptorFactory>();

        public List<StateMachineObserverFactory> ObserverFactories { get; set; } = new List<StateMachineObserverFactory>();

        [DebuggerHidden]
        public void Build()
        {
            Debug.Assert(InitialVertexName != null, $"Initial vertex name not assigned. Is state machine '{Name}' built properly?");

            if (Vertices.TryGetValue(InitialVertexName, out var initialVertex))
            {
                InitialVertex = initialVertex;
            }
            else
            {
                throw new StateMachineDefinitionException($"Initial state '{InitialVertexName}' is not registered in state machine '{Name}'");
            }

            foreach (var vertex in AllVertices.Values)
            {
                if (!string.IsNullOrEmpty(vertex.InitialVertexName))
                {
                    if (vertex.Vertices.TryGetValue(vertex.InitialVertexName, out initialVertex))
                    {
                        vertex.InitialVertex = initialVertex;
                    }
                    else
                    {
                        throw new StateMachineDefinitionException($"Initial state '{vertex.InitialVertexName}' is not registered in composite state '{vertex.Name}' in state machine '{Name}'");
                    }
                }

                var vertexTriggers = vertex.Edges
                    .Where(edge => !string.IsNullOrEmpty(edge.Trigger))
                    .Select(edge => edge.Trigger);

                foreach (var deferredEvent in vertex.DeferredEvents)
                {
                    if (vertexTriggers.Contains(deferredEvent))
                    {
                        throw new DeferralDefinitionException(deferredEvent, $"Event '{deferredEvent}' triggers a transition outgoing from state '{vertex.Name}' in state machine '{Name}' and cannot be deferred by that state.");
                    }
                }
            }

            foreach (var edge in AllEdges)
            {
                if (edge.TargetName != null && edge.TargetName != Constants.DefaultTransitionTarget)
                {
                    var vertices = edge.Source.Parent?.Vertices ?? Vertices;
                    if (vertices.TryGetValue(edge.TargetName, out var target))
                    {
                        edge.Target = target;
                    }
                    else
                    {
                        throw new TransitionDefinitionException(edge.Source.Parent is null
                            ? $"Transition target state '{edge.TargetName}' is not registered in root level of state machine '{Name}'"
                            : $"Transition target state '{edge.TargetName}' is not defined on the same level as transition source '{edge.SourceName}' in state machine '{Name}'"
                        );
                    }
                }
            }
        }
    }
}
