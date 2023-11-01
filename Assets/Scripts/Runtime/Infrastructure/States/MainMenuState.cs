using Core.Factories;
using Core.UI;
using UnityEngine;

namespace Core.Infrastructure
{
    public class MainMenuState : State
    {
        private readonly UIProvider _uiProvider;
        private MainMenu _mainMenu;

        public MainMenuState(UIProvider uiProvider)
        {
            _uiProvider = uiProvider;
        }

        public override void Enter()
        {
            _mainMenu = _uiProvider.CreateMainMenu();
        }

        public override void Exit()
        {
            Object.Destroy(_mainMenu.gameObject);
        }
    }
}
