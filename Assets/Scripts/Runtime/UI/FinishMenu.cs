using Core.Infrastructure;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class FinishMenu : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _menuButton;

        private GameNavigation _navigation;

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
        private void BaseConstruct(GameNavigation navigation) =>
            _navigation = navigation;

        private void RestartGame() => 
            _navigation.Regenerate<GameplayState>();

        private void ReturnToMenu() => 
            _navigation.Regenerate<MainMenuState>();
    }
}