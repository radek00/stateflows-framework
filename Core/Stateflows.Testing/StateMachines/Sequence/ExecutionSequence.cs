﻿using System;
using System.Collections.Generic;
using Stateflows.Common;
using Stateflows.StateMachines.Events;

namespace Stateflows.Testing.StateMachines.Sequence
{
    internal class ExecutionSequence : IExecutionSequenceBuilder
    {
        private List<string> Sequence = new List<string>();

        public void ValidateWith(IExecutionSequenceBuilder sequenceBuilder)
        {
            var actualSequence = (sequenceBuilder as ExecutionSequence).Sequence;

            int index = 0;
            for (int x = 0; x < Sequence.Count; x++)
            {
                var entry = Sequence[x];

                bool found = false;
                for (int i = index; i < actualSequence.Count; i++)
                {
                    if (actualSequence[i] == entry)
                    {
                        index = i + 1;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    throw new Exception($"Expected execution step \"{entry}\" not found");
                }
            }
        }

        public IExecutionSequenceBuilder DefaultTransitionEffect(string sourceStateName, string targetStateName)
            => TransitionEffect(EventInfo<Completion>.Name, sourceStateName, targetStateName);

        public IExecutionSequenceBuilder DefaultTransitionGuard(string sourceStateName, string targetStateName)
            => TransitionGuard(EventInfo<Completion>.Name, sourceStateName, targetStateName);

        public IExecutionSequenceBuilder InternalTransitionEffect(string eventName, string sourceStateName)
            => TransitionEffect(eventName, sourceStateName, "");

        public IExecutionSequenceBuilder InternalTransitionGuard(string eventName, string sourceStateName)
            => TransitionGuard(eventName, sourceStateName, "");

        public IExecutionSequenceBuilder StateEntry(string stateName)
        {
            Sequence.Add($"{stateName}::entry");
            return this;
        }

        public IExecutionSequenceBuilder StateExit(string stateName)
        {
            Sequence.Add($"{stateName}::exit");
            return this;
        }

        public IExecutionSequenceBuilder StateInitialize(string stateName)
        {
            Sequence.Add($"{stateName} ( o--initialize-->");
            return this;
        }

        public IExecutionSequenceBuilder StateMachineInitialize()
        {
            Sequence.Add($"--initialize-->");
            return this;
        }

        public IExecutionSequenceBuilder TransitionEffect(string eventName, string sourceStateName, string targetStateName)
        {
            Sequence.Add($"{sourceStateName}--{eventName}/effect-->{targetStateName}");
            return this;
        }

        public IExecutionSequenceBuilder TransitionGuard(string eventName, string sourceStateName, string targetStateName)
        {
            Sequence.Add($"{sourceStateName}--{eventName}/[guard]-->{targetStateName}");
            return this;
        }
    }
}
