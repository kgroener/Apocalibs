namespace Apocalibs.StateMachine.Contracts
{
    public interface IStateTransition<TState, TTrigger> where TState : struct where TTrigger : struct
    {
    }
}