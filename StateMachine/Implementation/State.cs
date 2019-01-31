using Apocalibs.StateMachine.Contracts;
using Apocalibs.StateMachine.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Apocalibs.StateMachine.Implementation
{
    internal class State<TState, TTrigger> : IStateBuilder<TState, TTrigger>, IImmutableState<TState, TTrigger> where TState : struct where TTrigger : struct
    {
        private readonly TState _innerState;
        private List<Transition<TState, TTrigger>> _allowedTransitions;
        private HashSet<TTrigger> _disallowedTriggers;
        private HashSet<TTrigger> _ignoredTriggers;

        private List<Func<Task>> _enterActions;
        private List<Func<Task>> _leaveActions;
        private bool _isConfigured;

        public State(TState innerState)
        {
            _innerState = innerState;
            _allowedTransitions = new List<Transition<TState, TTrigger>>();
            _disallowedTriggers = new HashSet<TTrigger>();
            _ignoredTriggers = new HashSet<TTrigger>();
            _enterActions = new List<Func<Task>>();
            _leaveActions = new List<Func<Task>>();
            _isConfigured = false;
        }

        internal IImmutableState<TState, TTrigger> FinishSetup()
        {
            _isConfigured = true;
            return this;
        }

        public IStateBuilder<TState, TTrigger> Allow(Func<ITransitionBuilder<TState, TTrigger>, IStateTransition<TState, TTrigger>> createTransition)
        {
            ValidateStateIsBeingSetup();

            var transition = (Transition<TState, TTrigger>)createTransition(new Transition<TState, TTrigger>(_innerState));
            _allowedTransitions.Add(transition);
            return this;
        }

        public IStateBuilder<TState, TTrigger> Disallow(TTrigger trigger)
        {
            ValidateStateIsBeingSetup();

            _disallowedTriggers.Add(trigger);
            return this;
        }

        public IStateBuilder<TState, TTrigger> Ignore(TTrigger trigger)
        {
            ValidateStateIsBeingSetup();

            _ignoredTriggers.Add(trigger);
            return this;
        }

        public IStateBuilder<TState, TTrigger> OnEnter(Action a)
        {
            return OnEnterAsync(() =>
            {
                a();
                return Task.CompletedTask;
            });
        }

        public IStateBuilder<TState, TTrigger> OnEnterAsync(Func<Task> a)
        {
            ValidateStateIsBeingSetup();

            _enterActions.Add(a);
            return this;
        }

        public IStateBuilder<TState, TTrigger> OnLeave(Action a)
        {
            return OnLeaveAsync(() =>
            {
                a();
                return Task.CompletedTask;
            });
        }

        public IStateBuilder<TState, TTrigger> OnLeaveAsync(Func<Task> a)
        {
            ValidateStateIsBeingSetup();

            _leaveActions.Add(a);
            return this;
        }


        public IStateChange<TState> HandleTrigger(TTrigger trigger)
        {
            ValidateStateHasBeenSetup();

            if (_disallowedTriggers.Contains(trigger) || (!_ignoredTriggers.Contains(trigger) && !_allowedTransitions.Any(t => t.Trigger.Equals(trigger))))
            {
                throw new InvalidTriggerException<TState, TTrigger>(_innerState, trigger);
            }

            var firstAllowedTransition = _allowedTransitions.FirstOrDefault(t => t.Trigger.Equals(trigger) && t.IsAllowed());

            if (_ignoredTriggers.Contains(trigger) || firstAllowedTransition == null)
            {
                return StateChange<TState>.None;
            }

            return new StateChange<TState>(firstAllowedTransition.To);
        }

        public async Task EnterStateAsync()
        {
            ValidateStateHasBeenSetup();

            foreach (var enterStateAction in _enterActions)
            {
                await enterStateAction();
            }
        }

        public async Task LeaveStateAsync()
        {
            ValidateStateHasBeenSetup();

            foreach (var leaveStateAction in _leaveActions)
            {
                await leaveStateAction();
            }
        }

        private void ValidateStateIsBeingSetup()
        {
            if (_isConfigured)
            {
                throw new InvalidOperationException("Cannot alter state behaviour once the FinishSetup has been called.");
            }
        }

        private void ValidateStateHasBeenSetup()
        {
            if (!_isConfigured)
            {
                throw new InvalidOperationException("State is not fully setup yet.");
            }
        }
    }
}
