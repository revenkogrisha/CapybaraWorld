using System;
using System.Collections.Generic;
using Core.Audio;
using Core.Common;
using Core.Factories;
using Core.Game;
using Core.Game.Input;
using Core.Level;
using Core.Other;
using Core.Player;
using Core.Saving;
using UnityEngine;
using Inject = Zenject.InjectAttribute;

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
        private readonly PlayerData _playerData;
        private readonly ISaveService _saveService;
        private readonly PlaythroughHandler _playthroughHandler;
        private readonly ParticlesHelper _particlesHelper;
        private readonly IAudioHandler _audioHandler;

        [Inject]
        public GameplayState(
            InputHandler inputHandler,
            ILevelGenerator levelGenerator,
            PlaythroughHandler playthroughHandler,
            PlayerFactory playerFactory,
            PlayerDeadlineFactory playerDeadlineFactory,
            IPlayerCamera playerCamera,
            Score score,
            UIProvider uiProvider,
            PlayerData playerData,
            ISaveService saveService,
            ParticlesHelper particlesHelper,
            IAudioHandler audioHandler)
        {
            _inputHandler = inputHandler;
            _levelGenerator = levelGenerator;
            _playthroughHandler = playthroughHandler;
            _playerFactory = playerFactory;
            _playerDeadlineFactory = playerDeadlineFactory;
            _playerCamera = playerCamera;
            _score = score;
            _uiProvider = uiProvider;
            _playerData = playerData;
            _saveService = saveService;
            _particlesHelper = particlesHelper;
            _audioHandler = audioHandler;
        }

        public override void Enter()
        {
            ClearDisposeLists();
            AddInjectedDisposables();

            Hero hero = CreateHero();            
            Transform heroTransform = hero.transform;

            _playerData.InitializeEvents(hero);

            _playerCamera.Initialize(hero);
            
            InitializeScoreCounter(heroTransform);
            InitializePlayerDeadline(heroTransform);
            _playthroughHandler.Initialize(hero);

            _levelGenerator.Initialize(heroTransform);
            _levelGenerator.SetActiveWorldCanvases(true);

            _particlesHelper.Initialize();

            CreateUI(hero);

            _inputHandler.Initialize();

            _audioHandler.PlaySound(AudioName.HeroSpawn);
        }

        public override void Exit()
        {
            _levelGenerator.SetActiveWorldCanvases(false);

            _saveService.Save();
            
            foreach (IDisposable disposable in _disposables)
                disposable.Dispose();

            // Made for simplicity - some objects could be just disabled & stored in special handler
            foreach (GameObject destroyable in _destroyables)
                destroyable.SelfDestroy();

            _disposables.Clear();
            _destroyables.Clear();
        }

        private void ClearDisposeLists()
        {
            _disposables.Clear();
            _destroyables.Clear();
        }

        private void AddInjectedDisposables()
        {
            _disposables.Add(_inputHandler);
            _disposables.Add(_playerCamera);
            _disposables.Add(_playthroughHandler);
            _disposables.Add(_levelGenerator);
            _disposables.Add(_playerData);
            _disposables.Add(_particlesHelper);
        }

        private Hero CreateHero()
        {
            Hero hero = _playerFactory.Create();

            _destroyables.Add(hero.gameObject);
            return hero;
        }

        private void InitializePlayerDeadline(Transform heroTransform)
        {
            FollowerObject playerDeadline = _playerDeadlineFactory.Create();
            playerDeadline.BeginFollowing(heroTransform);

            _disposables.Add(playerDeadline);
        }

        private void InitializeScoreCounter(Transform heroTransform)
        {
            IScoreCounter scoreCounter = new DistanceScoreCounter(heroTransform, _score);
            scoreCounter.StartCount();

            _disposables.Add(scoreCounter);
        }

        private void CreateUI(Hero hero)
        {
            CreateDashRecoveryDisplay(hero);
            
            if (_inputHandler is IInputUIHandler inputUIHandler)
                inputUIHandler.CreateUI();
        }

        private void CreateDashRecoveryDisplay(Hero hero)
        {
            DashRecoveryDisplay display = _uiProvider.CreateDashRecoveryDisplay();
            display.Initialize(hero);
            
            _destroyables.Add(display.gameObject);
        }
    }
}