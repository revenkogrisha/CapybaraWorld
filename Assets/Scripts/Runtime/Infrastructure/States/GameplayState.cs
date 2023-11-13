using System;
using System.Collections.Generic;
using Core.Factories;
using Core.Game;
using Core.Level;
using Core.Player;
using Core.UI;
using UnityEngine;
using Inject = Zenject.InjectAttribute;
using Object = UnityEngine.Object;

namespace Core.Infrastructure
{
    public class GameplayState : State
    {
        private readonly List<IDisposable> _disposables = new();
        private readonly List<GameObject> _destroyables = new();
        private readonly ILevelGenerator _levelGenerator;
        private readonly IFactory<Hero> _playerFactory;
        private readonly IFactory<FollowerObject> _playerDeadlineFactory;
        private readonly IPlayerCamera _playerCamera;
        private readonly Score _score;
        private readonly UIProvider _uiProvider;
        private Hero _hero;
        private GameOverHandler _gameOverHandler;

        [Inject]
        public GameplayState(
            ILevelGenerator levelGenerator,
            GameOverHandler gameOverHandler,
            PlayerFactory playerFactory,
            PlayerDeadlineFactory playerDeadlineFactory,
            IPlayerCamera playerCamera,
            Score score,
            UIProvider uiProvider)
        {
            _levelGenerator = levelGenerator;
            _gameOverHandler = gameOverHandler;
            _playerFactory = playerFactory;
            _playerDeadlineFactory = playerDeadlineFactory;
            _playerCamera = playerCamera;
            _score = score;
            _uiProvider = uiProvider;

        }

        public override void Enter()
        {
            _disposables.Clear();
            _destroyables.Clear();

            _disposables.Add(_playerCamera);
            _disposables.Add(_gameOverHandler);
            _disposables.Add(_levelGenerator);

            _hero = _playerFactory.Create();
            Transform heroTransform = _hero.transform;
            _destroyables.Add(_hero.gameObject);
            
            FollowerObject playerDeadline = _playerDeadlineFactory.Create();
            playerDeadline.BeginFollowing(heroTransform);
            _disposables.Add(playerDeadline);

            _gameOverHandler.SubscribeHeroDeath(_hero); // remake as interface

            IScoreCounter scoreCounter = new XPositionScoreCounter(heroTransform, _score);
            scoreCounter.StartCount();
            _disposables.Add(scoreCounter);

            _levelGenerator.InitializeCenter(heroTransform);
            _playerCamera.Initialize(_hero);
        }

        public override void Exit()
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            // Made for simplicity - some objects could be just disabled & stored somewhere
            foreach (GameObject destroyable in _destroyables)
                Object.Destroy(destroyable);

            _disposables.Clear();
            _destroyables.Clear();
        }
    }
}