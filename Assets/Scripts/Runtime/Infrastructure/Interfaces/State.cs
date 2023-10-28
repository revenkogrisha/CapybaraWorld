namespace Core.Infrastructure
{
    public abstract class State<TArg> : State
    {
        public abstract void SetArg(TArg arg);
    }

    public abstract class State
    {
        public FiniteStateMachine FiniteStateMachine { get; set; }

        public virtual void Enter() {  }
        public virtual void Update() {  }
        public virtual void Exit() {  }
    }
}
