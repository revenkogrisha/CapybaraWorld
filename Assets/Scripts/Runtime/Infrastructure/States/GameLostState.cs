using Core.Common;
using Core.Factories;
using Core.Other;
using Core.UI;
using Zenject;

namespace Core.Infrastructure
{
    public class GameLostState : State
    {
        private readonly UIProvider _uiProvider;
        private GameLostMenu _gameOverMenu;

        [Inject]
        public GameLostState(UIProvider uiProvider) => 
            _uiProvider = uiProvider;

        public override void Enter()
        {
            _gameOverMenu = _uiProvider.CreateGameLostMenu();
            
            HapticHelper.VibrateHeavy();
        }

        public override void Exit() => 
            _gameOverMenu.gameObject.SelfDestroy();
    }
}
