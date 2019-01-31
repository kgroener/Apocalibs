using System;
using System.Threading.Tasks;

namespace Apocalibs.StateMachine.Contracts
{
    public interface IStateBuilder<TState, TTrigger> where TState : struct where TTrigger : struct
    {
        IStateBuilder<TState, TTrigger> OnEnter(Action a);
        IStateBuilder<TState, TTrigger> OnEnterAsync(Func<Task> a);
        IStateBuilder<TState, TTrigger> OnLeave(Action a);
        IStateBuilder<TState, TTrigger> OnLeaveAsync(Func<Task> a);

        IStateBuilder<TState, TTrigger> Allow(Func<ITransitionBuilder<TState, TTrigger>, IStateTransition<TState, TTrigger>> state);
        //IState<TState, TTrigger> Disallow(Func<ITransition<TState, TTrigger>, IStateTransition<TState, TTrigger>> state);
        //IState<TState, TTrigger> Ignore(Func<ITransition<TState, TTrigger>, IStateTransition<TState, TTrigger>> state);

        IStateBuilder<TState, TTrigger> Disallow(TTrigger trigger);
        IStateBuilder<TState, TTrigger> Ignore(TTrigger trigger);
    }
}