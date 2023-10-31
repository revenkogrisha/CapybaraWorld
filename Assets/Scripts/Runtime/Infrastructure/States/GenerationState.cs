using Core.Factories;
using Core.Level;
using Core.Player;
using Zenject;

namespace Core.Infrastructure
{
    public class GenerationState : State
    {
        private readonly ILevelGenerator _levelGenerator;
        private readonly PlayerFactory _playerFactory;
        private readonly IPlayerCamera _playerCamera;

        [Inject]
        public GenerationState(
            ILevelGenerator levelGenerator,
            PlayerFactory playerFactory,
            IPlayerCamera camera)
        {
            _levelGenerator = levelGenerator;
            _playerFactory = playerFactory;
            _playerCamera = camera;
        }

        public override void Enter()
        {
            _levelGenerator.Generate();

            PlayerTest hero = _playerFactory.Create();
            _levelGenerator.ObservePlayer(hero);
            _playerCamera.Initialize(hero);
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
        }
    }
}
