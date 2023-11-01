using Core.Level;
using Zenject;

namespace Core.Infrastructure
{
    public class GenerationState : State
    {
        private readonly ILevelGenerator _levelGenerator;

        [Inject]
        public GenerationState(ILevelGenerator levelGenerator)
        {
            _levelGenerator = levelGenerator;
        }

        public override void Enter()
        {
            _levelGenerator.Generate();
            FiniteStateMachine.ChangeState<MainMenuState>();
        }
    }
}
