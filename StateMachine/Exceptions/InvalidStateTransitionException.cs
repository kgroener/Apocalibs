using System;
using System.Runtime.Serialization;

namespace Apocalibs.StateMachine.Exceptions
{
    [Serializable]
    public class InvalidStateTransitionException<TState> : Exception
    {
        public InvalidStateTransitionException(TState stateFrom, TState stateTo)
        {
        }

        public InvalidStateTransitionException(string message) : base(message)
        {
        }

        public InvalidStateTransitionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidStateTransitionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}