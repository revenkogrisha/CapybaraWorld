using Core.Infrastructure;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private UIButton _playButton;

        private GameNavigation _navigation;

        #region MonoBehaviour

        private void OnEnable() => 
            _playButton.OnClicked += StartGame;

        private void OnDisable() =>
            _playButton.OnClicked -= StartGame;

        #endregion

        [Inject]
        private void Construct(GameNavigation navigation)
        {
            _navigation = navigation;
        }

        private void StartGame() => 
            _navigation.ToGameplay();
    }
}