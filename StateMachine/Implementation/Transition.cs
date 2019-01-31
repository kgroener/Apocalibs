using Apocalibs.StateMachine.Contracts;
using System;

namespace Apocalibs.StateMachine.Implementation
{
    internal class Transition<TState, TTrigger> : ITransitionBuilder<TState, TTrigger>, IStateTransition<TState, TTrigger>, IConstrainedStateTransition<TState, TTrigger>, ITriggerTransition<TState, TTrigger> where TState : struct where TTrigger : struct
    {
        private Func<bool> _constraint;

        public Transition(TState stateFrom)
        {
            From = stateFrom;
            _isValidTransition = false;
        }

        public TState From { get; }

        private bool _isValidTransition;

        public TState To { get; private set; }

        public TTrigger Trigger { get; private set; }

        public IConstrainedStateTransition<TState, TTrigger> On(TTrigger trigger)
        {
            Trigger = trigger;
            return this;
        }

        public ITriggerTransition<TState, TTrigger> TransitionTo(TState state)
        {
            To = state;
            _isValidTransition = true;
            return this;
        }

        public IStateTransition<TState, TTrigger> When(Func<bool> expression)
        {
            _constraint = expression ?? throw new ArgumentNullException(nameof(expression));
            return this;
        }

        public bool IsValid => _isValidTransition;

        public bool IsAllowed()
        {
            return _constraint?.Invoke() ?? true;
        }
    }
}
