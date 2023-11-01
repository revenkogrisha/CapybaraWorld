using Core.Infrastructure;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UIButton _playButton;

        private IGlobalStateMachine _globalStateMachine;

        #region MonoBehaviour
        
        private void OnEnable()
        {
            _playButton.OnClicked += StartGame;
        }

        private void OnDisable()
        {
            _playButton.OnClicked -= StartGame;
        }

        #endregion

        [Inject]
        private void Construct(IGlobalStateMachine globalStateMachine)
        {
            _globalStateMachine = globalStateMachine;
        }

        private void StartGame()
        {
            _globalStateMachine.ChangeState<GameplayState>();
        }
    }
}
