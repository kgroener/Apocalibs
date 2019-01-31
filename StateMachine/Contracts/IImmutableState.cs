using System.Threading.Tasks;

namespace Apocalibs.StateMachine.Contracts
{
    internal interface IImmutableState<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        IStateChange<TState> HandleTrigger(TTrigger trigger);
        Task EnterStateAsync();
        Task LeaveStateAsync();
    }
}