using Core.Common;
using UnityEngine;
using Zenject;

namespace Core.Infrastructure
{
    public class Bootstrap : MonoBehaviour
    {
        private IGlobalStateMachine _stateMachine;
        private GenerationState _generationState;
        private MainMenuState _mainMenuState;
        private GameplayState _gameplayState;
        private GameWinState _gameWinState;
        private GameLostState _gameLostState;

        private void Awake()
        {
            AddGlobalStatesToMachine();
            _stateMachine.ChangeState<GenerationState, State>(_mainMenuState);
        }

        [Inject]
        private void Construct(
            IGlobalStateMachine stateMachine,
            GenerationState generationState,
            MainMenuState mainMenuState,
            GameplayState gameplayState,
            GameWinState gameWinState,
            GameLostState gameOverState)
        {
            _stateMachine = stateMachine;
            _generationState = generationState;
            _mainMenuState = mainMenuState;
            _gameplayState = gameplayState;
            _gameWinState = gameWinState;
            _gameLostState = gameOverState;
        }

        private void AddGlobalStatesToMachine()
        {
            _stateMachine.AddState<GenerationState>(_generationState);
            _stateMachine.AddState<MainMenuState>(_mainMenuState);
            _stateMachine.AddState<GameplayState>(_gameplayState);
            _stateMachine.AddState<GameWinState>(_gameWinState);
            _stateMachine.AddState<GameLostState>(_gameLostState);
        }
    }
}