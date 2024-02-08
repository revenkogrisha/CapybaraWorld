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
        private readonly UIProvider _uiProvider;
        private readonly MainMenu _mainMenu;
#if REVENKO_DEVELOP
        private readonly MainMenuDevHandler _mainMenuDev;
#endif
        private readonly HeroSkins _heroSkins;
        private MainMenuRoot _root;

        [Inject]
        public MainMenuState(
            UIProvider uiProvider,
            MainMenu mainMenu,
#if REVENKO_DEVELOP
            MainMenuDevHandler mainMenuDev,
#endif
            HeroSkins heroSkins)
        {
            _uiProvider = uiProvider;
            _mainMenu = mainMenu;
#if REVENKO_DEVELOP
            _mainMenuDev = mainMenuDev;
#endif
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
            _root.InitializeHeroMenu(heroMenu, new HeroMenuPresenter(_heroSkins, heroMenu));
        }
    }
}