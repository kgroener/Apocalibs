using System;
using System.Collections.Generic;
using System.Text;

namespace Apocalibs.StateMachine.Exceptions
{
    public class StateNotFoundException<TState> : Exception
    {
        public StateNotFoundException(TState state) : base($"State '{state}' was not found in the statemachine configuration.")
        {
            State = state;
        }

        public TState State { get; private set; }
    }
}
