#if UNITY_ANDROID && !UNITY_EDITOR
using Core.Common.Notifications;
using System.Threading;
using Core.Common;
using Core.Other;
using Cysharp.Threading.Tasks;
using Google.Play.AppUpdate;
#endif
using Core.Common.ThirdParty;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        private Notifications _notifications;
#endif
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
            FirebaseService.Initialize().Forget();
            
#if UNITY_ANDROID && !UNITY_EDITOR
            HandleAppUpdate().Forget();
            ScheduleAndroidNotifications();
#endif
            
            AddGameStatesToMachine();
            _navigation.ToLoadingData();
        }

        [Inject]
        private void Construct(
#if UNITY_ANDROID && !UNITY_EDITOR
            Notifications notifications,
#endif
            IGameStateMachine stateMachine,
            DataInitializationState dataInitializationState,
            GenerationState generationState,
            MainMenuState mainMenuState,
            GameplayState gameplayState,
            GameWinState gameWinState,
            GameLostState gameOverState,
            GameNavigation navigation)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            _notifications = notifications;
#endif
            _stateMachine = stateMachine;
            _dataInitializationState = dataInitializationState;
            _generationState = generationState;
            _mainMenuState = mainMenuState;
            _gameplayState = gameplayState;
            _gameWinState = gameWinState;
            _gameLostState = gameOverState;
            _navigation = navigation;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
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

        private void ScheduleAndroidNotifications()
        {
            _notifications.Send(_notifications.Collection.PlayReminder);
            _notifications.Send(_notifications.Collection.Locations);
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