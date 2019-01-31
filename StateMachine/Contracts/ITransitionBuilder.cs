namespace Apocalibs.StateMachine.Contracts
{
    public interface ITransitionBuilder<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        ITriggerTransition<TState, TTrigger> TransitionTo(TState state);
    }
}