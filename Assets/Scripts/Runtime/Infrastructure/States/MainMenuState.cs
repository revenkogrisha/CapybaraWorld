using Core.Common;
using Core.Factories;
using Core.Other;
using Core.Player;
using Core.UI;
using Zenject;

namespace Core.Infrastructure
{
    public class MainMenuState : State
    {
#if REVENKO_DEVELOP
        private readonly MainMenuDevHandler _mainMenuDev;
#endif
        private readonly UIProvider _uiProvider;
        private readonly MainMenu _mainMenu;
        private readonly PlayerUpgrade _playerUpgrade;
        private readonly HeroSkins _heroSkins;
        private readonly SettingsMenu _settingsMenu;
        private MainMenuRoot _root;

        [Inject]
        public MainMenuState(
#if REVENKO_DEVELOP
            MainMenuDevHandler mainMenuDev,
#endif
            PlayerUpgrade playerUpgrade,
            UIProvider uiProvider,
            MainMenu mainMenu,
            HeroSkins heroSkins,
            SettingsMenu settings)
        {
#if REVENKO_DEVELOP
            _mainMenuDev = mainMenuDev;
#endif
            _playerUpgrade = playerUpgrade;
            _uiProvider = uiProvider;
            _mainMenu = mainMenu;
            _heroSkins = heroSkins;
            _settingsMenu = settings;
        }

        public override void Enter()
        {
            if (_root == null)
            {
                _root = _uiProvider.CreateMainMenuRoot();

                InitializeRoot();
            }

            _root.SetActive(true);
            _root.ShowMainMenu();
        }

        public override void Exit() => 
            _root.SetActive(false);

        private void InitializeRoot() 
        {
            MainMenuView mainMenuView = _uiProvider.CreateMainMenu(_root.RectTransform);
                _root.InitializeMainMenu(mainMenuView, new MainMenuPresenter(
#if REVENKO_DEVELOP
                    _mainMenuDev,
#endif
                    _mainMenu));

            HeroMenuView heroMenuView = _uiProvider.CreateHeroMenu(_root.RectTransform);
            _root.InitializeHeroMenu(heroMenuView, new HeroMenuPresenter(
                _playerUpgrade,
                _heroSkins,
                heroMenuView));

            SettingsMenuView settingsMenuView = _uiProvider.CreateSettingsMenu(_root.RectTransform);
            _root.InitializeSettingsMenu(settingsMenuView, new SettingsMenuPresenter(
                _settingsMenu,
                settingsMenuView));
        }
    }
}