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
        private readonly HeroSkins _heroSkins;
        private MainMenuRoot _root;

        [Inject]
        public MainMenuState(UIProvider uiProvider, HeroSkins heroSkins)
        {
            _uiProvider = uiProvider;
            _heroSkins = heroSkins;
        }

        public override void Enter()
        {
            if (_root == null)
            {
                _root = _uiProvider.CreateMainMenuRoot();
                _root.InitializeMainMenu(_uiProvider.CreateMainMenu());

                HeroMenu heroMenu = _uiProvider.CreateHeroMenu();
                _root.InitializeHeroMenu(heroMenu, new HeroMenuPresenter(_heroSkins, heroMenu));
            }

            _root.SetActive(true);
            _root.ShowMainMenu();
        }

        public override void Exit() => 
            _root.SetActive(false);
    }
}