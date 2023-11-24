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
        private GameOverState _gameOverState;

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
            GameOverState gameOverState)
        {
            _stateMachine = stateMachine;
            _generationState = generationState;
            _mainMenuState = mainMenuState;
            _gameplayState = gameplayState;
            _gameOverState = gameOverState;
        }

        private void AddGlobalStatesToMachine()
        {
            _stateMachine.AddState<GenerationState>(_generationState);
            _stateMachine.AddState<MainMenuState>(_mainMenuState);
            _stateMachine.AddState<GameplayState>(_gameplayState);
            _stateMachine.AddState<GameOverState>(_gameOverState);
        }
    }
}