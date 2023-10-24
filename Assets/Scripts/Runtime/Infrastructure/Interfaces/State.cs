namespace Core.Infrastructure
{
    public abstract class State<TArg> : State
    {
        public abstract void SetArg(TArg arg);
    }

    public abstract class State
    {
        public FiniteStateMachine FiniteStateMachine { get; set; }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}
