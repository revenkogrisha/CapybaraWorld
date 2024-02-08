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
        private MainMenuRoot _root;

        [Inject]
        public MainMenuState(
#if REVENKO_DEVELOP
            MainMenuDevHandler mainMenuDev,
#endif
            PlayerUpgrade playerUpgrade,
            UIProvider uiProvider,
            MainMenu mainMenu,
            HeroSkins heroSkins)
        {
#if REVENKO_DEVELOP
            _mainMenuDev = mainMenuDev;
#endif
            _playerUpgrade = playerUpgrade;
            _uiProvider = uiProvider;
            _mainMenu = mainMenu;
            _heroSkins = heroSkins;
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
            MainMenuView mainMenu = _uiProvider.CreateMainMenu(_root.RectTransform);
                _root.InitializeMainMenu(mainMenu, new MainMenuPresenter(
#if REVENKO_DEVELOP
                    _mainMenuDev,
#endif
                    _mainMenu));

            HeroMenuView heroMenu = _uiProvider.CreateHeroMenu(_root.RectTransform);
            _root.InitializeHeroMenu(heroMenu, new HeroMenuPresenter(_playerUpgrade, _heroSkins, heroMenu));
        }
    }
}