using Core.Factories;
using Core.Level;
using Core.Player;

namespace Core.Infrastructure
{
    public class GameplayState : State
    {
        private readonly ILevelGenerator _levelGenerator;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerDeadlineFactory _playerDeadlineFactory;
        private readonly IPlayerCamera _playerCamera;
        
        public GameplayState(
            ILevelGenerator levelGenerator,
            PlayerFactory playerFactory,
            PlayerDeadlineFactory playerDeadlineFactory,
            IPlayerCamera playerCamera)
        {
            _levelGenerator = levelGenerator;
            _playerFactory = playerFactory;
            _playerDeadlineFactory = playerDeadlineFactory;
            _playerCamera = playerCamera;
        }

        public override void Enter()
        {
            PlayerTest hero = _playerFactory.Create();
            FollowerObject deadline = _playerDeadlineFactory.Create(hero.transform);
            deadline.BeginFollowing();

            _levelGenerator.ObservePlayer(hero);
            _playerCamera.Initialize(hero);
        }
    }
}
