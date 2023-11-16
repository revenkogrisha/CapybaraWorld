using System;
using System.Collections.Generic;
using Core.Factories;
using Core.Game;
using Core.Game.Input;
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
        private readonly InputHandler _inputHandler;
        private readonly ILevelGenerator _levelGenerator;
        private readonly IFactory<Hero> _playerFactory;
        private readonly IFactory<FollowerObject> _playerDeadlineFactory;
        private readonly IPlayerCamera _playerCamera;
        private readonly Score _score;
        private readonly UIProvider _uiProvider;
        private GameOverHandler _gameOverHandler;

        [Inject]
        public GameplayState(
            InputHandler inputHandler,
            ILevelGenerator levelGenerator,
            GameOverHandler gameOverHandler,
            PlayerFactory playerFactory,
            PlayerDeadlineFactory playerDeadlineFactory,
            IPlayerCamera playerCamera,
            Score score,
            UIProvider uiProvider)
        {
            _inputHandler = inputHandler;
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
            ClearLists();
            AddInjectedDisposables();
            
            _inputHandler.Initialize();

            Hero hero = CreateHero();            
            Transform heroTransform = hero.transform;

            InitializeCamera(hero);
            
            InitializeScoreCounter(heroTransform);
            InitializePlayerDeadline(heroTransform);
            InitializeGameOverHandler(hero);

            _levelGenerator.InitializeCenter(heroTransform);

            DisplayScore();
        }

        public override void Exit()
        {
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            // Made for simplicity - some objects could be just disabled & stored in special handler
            foreach (GameObject destroyable in _destroyables)
                Object.Destroy(destroyable);

            _disposables.Clear();
            _destroyables.Clear();
        }

        private void ClearLists()
        {
            _disposables.Clear();
            _destroyables.Clear();
        }

        private void AddInjectedDisposables()
        {
            _disposables.Add(_inputHandler);
            _disposables.Add(_playerCamera);
            _disposables.Add(_gameOverHandler);
            _disposables.Add(_levelGenerator);
        }

        private Hero CreateHero()
        {
            Hero hero = _playerFactory.Create();
            _destroyables.Add(hero.gameObject);
            return hero;
        }

        private void InitializeCamera(Hero hero) =>
            _playerCamera.Initialize(hero);

        private void InitializePlayerDeadline(Transform heroTransform)
        {
            FollowerObject playerDeadline = _playerDeadlineFactory.Create();
            playerDeadline.BeginFollowing(heroTransform);
            _disposables.Add(playerDeadline);
        }

        private void InitializeGameOverHandler(IDieable hero) =>
            _gameOverHandler.SubscribeHeroDeath(hero);

        private void InitializeScoreCounter(Transform heroTransform)
        {
            IScoreCounter scoreCounter = new DistanceScoreCounter(heroTransform, _score);
            scoreCounter.StartCount();
            _disposables.Add(scoreCounter);
        }

        private void DisplayScore()
        {
            ScoreDisplay scoreDisplay = _uiProvider.CreateScoreDisplay();
            _destroyables.Add(scoreDisplay.gameObject);
        }
    }
}