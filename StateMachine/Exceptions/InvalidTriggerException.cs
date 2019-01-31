using System;

namespace Apocalibs.StateMachine.Exceptions
{
    public class InvalidTriggerException<TState, TTrigger> : Exception
    {
        public InvalidTriggerException(TState state, TTrigger trigger) : base($"The state '{state}' does not allow a trigger of '{trigger}'.")
        {

        }
    }
}
