﻿using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Stateflows.StateMachines.Context.Interfaces;

namespace Stateflows.StateMachines
{
    public static class StateAction
    {
        [DebuggerHidden]
        public static Func<IStateActionContext, Task> ToAsync(this Action<IStateActionContext> stateAction)
            => c => Task.Run(() => stateAction(c));
    }
}
