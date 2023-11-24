namespace Core.Common
{
    public interface IFiniteStateMachine
    {
        public void AddState<TState>(State state) 
            where TState : State;

        public void ChangeState<TState>() 
            where TState : State;

        public void ChangeState<TState, TArg>(TArg arg) 
            where TState : State<TArg>;

        public void ChangeState(State state);

        public void ChangeState<TArg>(State<TArg> state, TArg arg);

        public TState GetStateWithArg<TState, TArg>(TArg arg) 
            where TState : State<TArg>;

        public TState GetState<TState>() 
            where TState : State;

        public void DoStateUpdate();

        public bool CompareState<TState>();
    }
}
