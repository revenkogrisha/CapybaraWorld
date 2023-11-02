using Core.Factories;
using Core.Common;
using Core.Level;
using Core.Player;
using UnityEngine;

namespace Core.Infrastructure
{
    public class GameplayState : State
    {
        private readonly ILevelGenerator _levelGenerator;
        private readonly PlayerFactory _playerFactory;
        private readonly PlayerDeadlineFactory _playerDeadlineFactory;
        private readonly IPlayerCamera _playerCamera;
        private PlayerTest _hero;
        private FollowerObject _playerDeadline;
        private GameOverHandler _gameOverHandler;
        
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
            _playerDeadline = _playerDeadlineFactory.Create(_hero.transform);
            _playerDeadline.BeginFollowing();

            _gameOverHandler.SubscribeHeroDeath(_hero);

            _levelGenerator.ObservePlayer(_hero);
            _playerCamera.Initialize(_hero);
        }

        public override void Exit()
        {
            Object.Destroy(_hero.gameObject);
            Object.Destroy(_playerDeadline.gameObject);

            _gameOverHandler.Dispose();
            _levelGenerator.Dispose();
        }
    }
}