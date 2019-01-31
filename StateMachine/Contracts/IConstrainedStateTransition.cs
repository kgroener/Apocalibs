using System;

namespace Apocalibs.StateMachine.Contracts
{
    public interface IConstrainedStateTransition<TState, TTrigger> : IStateTransition<TState, TTrigger> where TState : struct where TTrigger : struct
    {
        IStateTransition<TState, TTrigger> When(Func<bool> expression);
    }
}