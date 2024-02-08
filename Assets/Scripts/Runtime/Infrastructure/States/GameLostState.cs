using Core.Audio;
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
        private readonly IAudioHandler _audioHandler;
        private GameLostMenu _gameOverMenu;

        [Inject]
        public GameLostState(UIProvider uiProvider, IAudioHandler audioHandler)
        {
            _uiProvider = uiProvider;
            _audioHandler = audioHandler;
        }

        public override void Enter()
        {
            _audioHandler.PlaySound(AudioName.GameLost);
            
            _gameOverMenu = _uiProvider.CreateGameLostMenu();
            
            HapticHelper.VibrateHeavy();
        }

        public override void Exit() => 
            _gameOverMenu.gameObject.SelfDestroy();
    }
}
