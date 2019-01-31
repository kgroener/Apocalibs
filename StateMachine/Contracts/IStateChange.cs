namespace Apocalibs.StateMachine.Contracts
{
    internal interface IStateChange<TState> where TState : struct
    {
        TState? NextState { get; }
        bool NoChange { get; }
    }
}