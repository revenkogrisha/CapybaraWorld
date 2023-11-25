using Core.Common;
using Core.Infrastructure;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class FinalMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _menuButton;

        private IGlobalStateMachine _globalStateMachine;

        private void OnEnable()
        {
            _restartButton.OnClicked += RestartGame;
            _menuButton.OnClicked += ReturnToMenu;
        }

        private void OnDisable()
        {
            _restartButton.OnClicked -= RestartGame;
            _menuButton.OnClicked -= ReturnToMenu;
        }

        [Inject]
        private void BaseConstruct(IGlobalStateMachine globalStateMachine) =>
            _globalStateMachine = globalStateMachine;

        private void RestartGame()
        {
            GameplayState gameplayState = _globalStateMachine.GetState<GameplayState>();
            GenerationState generationState = _globalStateMachine
                .GetStateWithArg<GenerationState, State>(gameplayState);

            _globalStateMachine.ChangeState(generationState);
        }

        private void ReturnToMenu()
        {
            MainMenuState mainMenuState = _globalStateMachine.GetState<MainMenuState>();
            GenerationState generationState = _globalStateMachine
                .GetStateWithArg<GenerationState, State>(mainMenuState);

            _globalStateMachine.ChangeState(generationState);
        }
    }
}