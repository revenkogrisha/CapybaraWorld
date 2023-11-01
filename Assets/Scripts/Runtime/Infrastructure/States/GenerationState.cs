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
        private readonly PlayerDeadlineFactory _playerDeadlineFactory;
        private readonly IPlayerCamera _playerCamera;

        [Inject]
        public GenerationState(
            ILevelGenerator levelGenerator,
            PlayerFactory playerFactory,
            PlayerDeadlineFactory playerDeadlineFactory,
            IPlayerCamera camera)
        {
            _levelGenerator = levelGenerator;
            _playerFactory = playerFactory;
            _playerDeadlineFactory = playerDeadlineFactory;
            _playerCamera = camera;
        }

        public override void Enter()
        {
            PlayerTest hero = _playerFactory.Create();
            FollowerObject deadline = _playerDeadlineFactory.Create(hero.transform);
            deadline.BeginFollowing();

            _levelGenerator.Generate();
            _levelGenerator.ObservePlayer(hero);

            _playerCamera.Initialize(hero);
        }

        public override void Exit()
        {
        }
    }
}
