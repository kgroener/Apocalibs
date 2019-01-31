namespace Apocalibs.StateMachine.Contracts
{
    public interface ITriggerTransition<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        IConstrainedStateTransition<TState, TTrigger> On(TTrigger trigger);
    }
}