using Core.Infrastructure;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _menuButton;

        private IGlobalStateMachine _globalStateMachine;

        #region MonoBehaviour
        
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

        #endregion

        [Inject]
        private void Construct(IGlobalStateMachine globalStateMachine)
        {
            _globalStateMachine = globalStateMachine;
        }

        private void RestartGame()
        {
            _globalStateMachine.ChangeState<GenerationState>();
            _globalStateMachine.ChangeState<GameplayState>();
        }

        private void ReturnToMenu()
        {
            _globalStateMachine.ChangeState<MainMenuState>();
        }
    }
}
