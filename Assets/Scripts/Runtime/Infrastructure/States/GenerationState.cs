using Core.Common;
using Core.Factories;
using Core.Level;
using Core.UI;
using Zenject;

namespace Core.Infrastructure
{
    public class GenerationState : State<State>
    {
        private readonly ILevelGenerator _levelGenerator;
        private readonly UIProvider _uiProvider;
        private State _stateToMoveOn;

        [Inject]
        public GenerationState(ILevelGenerator levelGenerator, UIProvider uiProvider)
        {
            _levelGenerator = levelGenerator;
            _uiProvider = uiProvider;
        }

        public override void Enter()
        {
            AreaLabelContainer container = _uiProvider.CreateAreaLabelContainer();
            _levelGenerator.InitializeLabels(container);
            
            _levelGenerator.Clean();
            _levelGenerator.Generate();
            FiniteStateMachine.ChangeState(_stateToMoveOn);
        }

        public override void SetArg(State stateToMoveOn) =>
            _stateToMoveOn = stateToMoveOn;
    }
}