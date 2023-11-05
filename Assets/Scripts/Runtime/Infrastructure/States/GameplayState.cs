using Core.Factories;
using Core.Common;
using Core.Level;
using Core.Player;
using UnityEngine;
using Inject = Zenject.InjectAttribute;

namespace Core.Infrastructure
{
    public class GameplayState : State
    {
        private readonly ILevelGenerator _levelGenerator;
        private readonly IFactory<Hero> _playerFactory;
        private readonly IFactory<FollowerObject> _playerDeadlineFactory;
        private readonly IPlayerCamera _playerCamera;
        private Hero _hero;
        private FollowerObject _playerDeadline;
        private GameOverHandler _gameOverHandler;
        
        [Inject]
        public GameplayState(
            ILevelGenerator levelGenerator,
            GameOverHandler gameOverHandler,
            PlayerFactory playerFactory,
            PlayerDeadlineFactory playerDeadlineFactory,
            IPlayerCamera playerCamera)
        {
            _levelGenerator = levelGenerator;
            _gameOverHandler = gameOverHandler;
            _playerFactory = playerFactory;
            _playerDeadlineFactory = playerDeadlineFactory;
            _playerCamera = playerCamera;
        }

        public override void Enter()
        {
            _hero = _playerFactory.Create();
            
            _playerDeadline = _playerDeadlineFactory.Create();
            _playerDeadline.BeginFollowing(_hero.transform);

            _gameOverHandler.SubscribeHeroDeath(_hero);

            _levelGenerator.InitializeCenter(_hero.transform);
            _playerCamera.Initialize(_hero);
        }

        public override void Exit()
        {
            Object.Destroy(_hero.gameObject);

            _playerDeadline.Dispose();
            _gameOverHandler.Dispose();
            _levelGenerator.Dispose();
        }
    }
}