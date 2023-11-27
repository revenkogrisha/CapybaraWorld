using Core.Common;
using Core.Factories;
using Core.Other;
using Core.UI;
using Zenject;

namespace Core.Infrastructure
{
    public class MainMenuState : State
    {
        private readonly UIProvider _uiProvider;
        private MainMenu _mainMenu;

        [Inject]
        public MainMenuState(UIProvider uiProvider) =>
            _uiProvider = uiProvider;

        public override void Enter() =>
            _mainMenu = _uiProvider.CreateMainMenu();

        public override void Exit() =>
            _mainMenu.gameObject.SelfDestroy();
    }
}