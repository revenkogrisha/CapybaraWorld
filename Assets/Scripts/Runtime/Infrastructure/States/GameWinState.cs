using Core.Common;
using Core.Factories;
using Core.Other;
using Core.UI;
using Zenject;

namespace Core.Infrastructure
{
    public class GameWinState : State
    {
        private readonly UIProvider _uiProvider;
        private GameWinMenu _gameWinMenu;

        [Inject]
        public GameWinState(UIProvider uiProvider) => 
            _uiProvider = uiProvider;

        public override void Enter() => 
            _gameWinMenu = _uiProvider.CreateGameWinMenu();

        public override void Exit() => 
            _gameWinMenu.gameObject.SelfDestroy();
    }
}