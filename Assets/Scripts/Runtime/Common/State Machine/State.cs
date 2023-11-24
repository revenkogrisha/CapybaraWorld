namespace Core.Common
{
    public abstract class State<TArg> : State
    {
        public abstract void SetArg(TArg arg);
    }

    public abstract class State
    {
        public IFiniteStateMachine FiniteStateMachine { get; set; }

        public virtual void Enter() {  }
        public virtual void Update() {  }
        public virtual void Exit() {  }
    }
}
