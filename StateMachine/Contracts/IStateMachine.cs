using System;
using System.Threading.Tasks;

namespace Apocalibs.StateMachine.Contracts
{
    public interface IStateMachine<TState, TTrigger>
        where TState : struct
        where TTrigger : struct
    {
        Task<bool> SendTriggerAsync(TTrigger trigger);

        TState CurrentState { get; }

        event EventHandler StateChanged;
    }
}