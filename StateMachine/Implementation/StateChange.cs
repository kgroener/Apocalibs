using Apocalibs.StateMachine.Contracts;

namespace Apocalibs.StateMachine.Implementation
{
    internal class StateChange<TState> : IStateChange<TState> where TState : struct
    {
        private readonly bool _noChange;
        private static readonly StateChange<TState> _ignoredStateChange;

        static StateChange()
        {
            _ignoredStateChange = new StateChange<TState>(true);
        }

        private StateChange(bool noChange)
        {
            _noChange = noChange;
        }

        public StateChange(TState nextState)
        {
            NextState = nextState;
        }

        public static StateChange<TState> None => _ignoredStateChange;


        public TState? NextState { get; private set; }

        public bool NoChange => _noChange;
    }
}
