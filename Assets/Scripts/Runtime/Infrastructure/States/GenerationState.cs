using Core.Level;
using Core.Player;

namespace Core.Infrastructure
{
    public class GenerationState : State
    {
        private readonly ILevelGenerator _levelGenerator;
        private readonly PlayerTest _hero;

        public GenerationState(ILevelGenerator levelGenerator, PlayerTest hero)
        {
            _hero = hero;
            _levelGenerator = levelGenerator;
        }

        public override void Enter()
        {
            _levelGenerator.Generate();
            _levelGenerator.ObservePlayer(_hero);
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }
    }
}
