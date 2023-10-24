using System;
using System.Collections.Generic;

namespace Core.Infrastructure
{
    public class FiniteStateMachine : IFiniteStateMachine
    {
        private readonly Dictionary<Type, State> _states;
        private State _currentState;

        public FiniteStateMachine()
        {
            _states = new();
        }

        public void AddState<TState>(State state)
            where TState : State
        {
            Type type = typeof(TState);
            _states[type] = state;
        }

        public void ChangeState<TState>()
            where TState : State
        {
            Type type = typeof(TState);
            if (_states.ContainsKey(type) == false)
                throw new ArgumentException($"Unable to change to {type.FullName} - it's not added to the state machine! Add it first");

            State newState = _states[type];
            newState.FiniteStateMachine = this;

            _currentState?.Exit();
            _currentState = newState;

            newState.Enter();
        }


        public TState GetStateWithArg<TState, TArg>(TArg arg) where TState : State<TArg>
        {
            State<TArg> state = _states[typeof(TState)] as State<TArg>;
            state.SetArg(arg);
            
            return state as TState;
        }

        public TState GetState<TState>() where TState : State => _states[typeof(TState)] as TState;

        public void DoStateUpdate() => _currentState.Update();

        public bool CompareState<TState>() => _currentState.GetType() == typeof(TState);
    }
}