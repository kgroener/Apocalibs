using Apocalibs.StateMachine.Contracts;
using Apocalibs.StateMachine.Exceptions;
using Apocalibs.StateMachine.Implementation;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Apocalibs.StateMachine
{
    /*
     * 
     * 
     * 
     * 
     */


    public class StateMachine
    {
        public static IStateMachineBuilder<TState, TTrigger> SetupNew<TState, TTrigger>() where TState : struct where TTrigger : struct
        {
            return new StateMachineBuilder<TState, TTrigger>();
        }
    }

    internal class StateMachine<TState, TTrigger> : IStateMachine<TState, TTrigger> where TState : struct where TTrigger : struct
    {
        private readonly IReadOnlyDictionary<TState, IImmutableState<TState, TTrigger>> _states;
        private TState _currentState;
        private SemaphoreSlim _stateLock;

        public StateMachine(IReadOnlyDictionary<TState, IImmutableState<TState, TTrigger>> states, TState initialState)
        {
            _states = states;

            ValidateStateExists(initialState);

            _currentState = initialState;
            _stateLock = new SemaphoreSlim(1, 1);
        }

        private void ValidateStateExists(TState state)
        {
            if (!_states.ContainsKey(state))
            {
                throw new StateNotFoundException<TState>(state);
            }
        }

        public TState CurrentState => _currentState;

        public event EventHandler StateChanged;

        public async Task<bool> SendTriggerAsync(TTrigger trigger)
        {
            await _stateLock.WaitAsync();
            try
            {
                var currentStateConfiguration = _states[_currentState];
                var stateChange = currentStateConfiguration.HandleTrigger(trigger);

                if (stateChange.NoChange)
                {
                    return false;
                }

                ValidateStateExists(stateChange.NextState.Value);

                await currentStateConfiguration.LeaveStateAsync();
                _currentState = stateChange.NextState.Value;
                currentStateConfiguration = _states[_currentState];
                await currentStateConfiguration.EnterStateAsync();

            }
            finally
            {
                _stateLock.Release();
            }

            StateChanged?.Invoke(this, EventArgs.Empty);
            return true;
        }
    }


    internal class StateMachineBuilder<TState, TTrigger> : IStateMachineBuilder<TState, TTrigger> where TState : struct where TTrigger : struct
    {
        private Dictionary<TState, IImmutableState<TState, TTrigger>> _configuredStates;

        public StateMachineBuilder()
        {
            _configuredStates = new Dictionary<TState, IImmutableState<TState, TTrigger>>();
        }

        public IStateMachineBuilder<TState, TTrigger> AddState(TState state, Func<IStateBuilder<TState, TTrigger>, IStateBuilder<TState, TTrigger>> stateBuilder)
        {
            if (_configuredStates.ContainsKey(state))
            {
                throw new InvalidOperationException("Duplicate state declaration.");
            }

            var configuredState = (State<TState, TTrigger>)stateBuilder(new State<TState, TTrigger>(state));
            var immuatableState = configuredState.FinishSetup();
            _configuredStates.Add(state, immuatableState);

            return this;
        }

        public IStateMachine<TState, TTrigger> FinishSetup(TState initialState)
        {
            return new StateMachine<TState, TTrigger>(_configuredStates, initialState);
        }
    }
}
