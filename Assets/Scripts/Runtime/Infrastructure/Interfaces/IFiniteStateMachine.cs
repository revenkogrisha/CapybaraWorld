namespace Core.Infrastructure
{
    public interface IFiniteStateMachine
    {
        public void DoStateUpdate();

        public bool CompareState<TState>();
    }
}
