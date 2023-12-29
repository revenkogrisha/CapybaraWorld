#if !UNITY_EDITOR && UNITY_ANDROID
using System.Threading;
using Core.Common;
using Core.Other;
using Cysharp.Threading.Tasks;
using Google.Play.AppUpdate;
#endif
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private IGameStateMachine _stateMachine;
        private DataInitializationState _dataInitializationState;
        private GenerationState _generationState;
        private MainMenuState _mainMenuState;
        private GameplayState _gameplayState;
        private GameWinState _gameWinState;
        private GameLostState _gameLostState;
        private GameNavigation _navigation;

        private void Awake()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            HandleAppUpdate().Forget();
#endif
            
            AddGameStatesToMachine();
            _navigation.ToLoadingData();
        }

        [Inject]
        private void Construct(
            IGameStateMachine stateMachine,
            DataInitializationState dataInitializationState,
            GenerationState generationState,
            MainMenuState mainMenuState,
            GameplayState gameplayState,
            GameWinState gameWinState,
            GameLostState gameOverState,
            GameNavigation navigation)
        {
            _stateMachine = stateMachine;
            _dataInitializationState = dataInitializationState;
            _generationState = generationState;
            _mainMenuState = mainMenuState;
            _gameplayState = gameplayState;
            _gameWinState = gameWinState;
            _gameLostState = gameOverState;
            _navigation = navigation;
        }

#if !UNITY_EDITOR && UNITY_ANDROID
        private async UniTaskVoid HandleAppUpdate()
        {
            const float requestTimeout = 60f;
            CancellationTokenSource cts = new();

            IAppUpdateService updateService = new AppUpdateService();
            updateService.Initialize();
            
            cts.CancelByTimeout(requestTimeout).Forget();
            UpdateAvailability result = await updateService.StartUpdateCheck(cts.Token);

            if (result == UpdateAvailability.UpdateAvailable)
                await updateService.Request(cts.Token);
        }
#endif

        private void AddGameStatesToMachine()
        {
            _stateMachine.AddState<DataInitializationState>(_dataInitializationState);
            _stateMachine.AddState<GenerationState>(_generationState);
            _stateMachine.AddState<MainMenuState>(_mainMenuState);
            _stateMachine.AddState<GameplayState>(_gameplayState);
            _stateMachine.AddState<GameWinState>(_gameWinState);
            _stateMachine.AddState<GameLostState>(_gameLostState);
        }
    }
}