namespace Core.Infrastructure
{
    public interface IFiniteStateMachine
    {
        public void AddState<TState>(State state) where TState : State;

        public void ChangeState<TState>() where TState : State;

        public void DoStateUpdate();

        public bool CompareState<TState>();
    }
}
