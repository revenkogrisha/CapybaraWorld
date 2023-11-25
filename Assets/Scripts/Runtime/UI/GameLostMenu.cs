using Core.Common;
using Core.Game;
using Core.Infrastructure;
using UnityEngine;
using UnityEngine.UI;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class GameLostMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _menuButton;

        [Space]
        [SerializeField] private Slider _progressBar;

        private IGlobalStateMachine _globalStateMachine;
        private IPlaythroughProgressHandler _playthrough;

        #region MonoBehaviour

        private void OnEnable()
        {
            _restartButton.OnClicked += RestartGame;
            _menuButton.OnClicked += ReturnToMenu;
            
            SetProgressBarValue(_playthrough.PlaythroughProgress);
        }

        private void OnDisable()
        {
            _restartButton.OnClicked -= RestartGame;
            _menuButton.OnClicked -= ReturnToMenu;
        }

        #endregion

        [Inject]
        private void Construct(
            IGlobalStateMachine globalStateMachine,
            IPlaythroughProgressHandler playthrough, Score score)
        {
            _globalStateMachine = globalStateMachine;
            _playthrough = playthrough;
        }

        private void SetProgressBarValue(float delta) => 
            _progressBar.value = delta;

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
