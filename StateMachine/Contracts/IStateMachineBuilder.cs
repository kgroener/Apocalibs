using System;

namespace Apocalibs.StateMachine.Contracts
{
    public interface IStateMachineBuilder<TState, TTrigger> where TState : struct where TTrigger : struct
    {
        IStateMachineBuilder<TState, TTrigger> AddState(TState state, Func<IStateBuilder<TState, TTrigger>, IStateBuilder<TState, TTrigger>> stateBuilder);
        IStateMachine<TState, TTrigger> FinishSetup(TState initialState);
    }
}