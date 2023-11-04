using Core.Level;
using Zenject;

namespace Core.Infrastructure
{
    public class GenerationState : State<State>
    {
        private readonly ILevelGenerator _levelGenerator;
        private State _stateToMoveOn;

        [Inject]
        public GenerationState(ILevelGenerator levelGenerator) => 
            _levelGenerator = levelGenerator;

        public override void Enter()
        {
            _levelGenerator.Generate();
            FiniteStateMachine.ChangeState(_stateToMoveOn);
        }

        public override void SetArg(State stateToMoveOn) =>
        _stateToMoveOn = stateToMoveOn;

    }
}
