using Core.Common;
using Core.Factories;
using Core.Mediation;
using Core.Other;
using Core.UI;
using Zenject;

namespace Core.Infrastructure
{
    public class GameLostState : State
    {
        private readonly UIProvider _uiProvider;
        private readonly IMediationService _mediationService;
        private GameLostMenu _gameOverMenu;

        [Inject]
        public GameLostState(UIProvider uiProvider, IMediationService mediationService)
        {
            _uiProvider = uiProvider;
            _mediationService = mediationService;
        }

        public override void Enter()
        {
            _gameOverMenu = _uiProvider.CreateGameLostMenu();

            _mediationService.ShowInterstitial();
            
            HapticHelper.VibrateHeavy();
        }

        public override void Exit() => 
            _gameOverMenu.gameObject.SelfDestroy();
    }
}
