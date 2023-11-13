using Core.Factories;
using Core.Game;
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
        private IScoreCounter _scoreCounter;

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
            Transform heroTransform = _hero.transform;
            
            _playerDeadline = _playerDeadlineFactory.Create();
            _playerDeadline.BeginFollowing(heroTransform);

            _gameOverHandler.SubscribeHeroDeath(_hero); // as interface

            Score score = new();
            _scoreCounter = new YPositionScoreCounter(heroTransform, score);

            _levelGenerator.InitializeCenter(_hero.transform);
            _playerCamera.Initialize(_hero);

            
        }

        public override void Exit()
        {
            _playerCamera.Dispose();
            _playerDeadline.Dispose();
            _gameOverHandler.Dispose();
            _levelGenerator.Dispose();
            _scoreCounter.Dispose();

            Object.Destroy(_hero.gameObject);
        }
    }
}