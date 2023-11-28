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