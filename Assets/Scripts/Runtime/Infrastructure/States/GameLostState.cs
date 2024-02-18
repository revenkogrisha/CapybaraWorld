using Core.Audio;
using Core.Common;
using Core.Factories;
using Core.Game;
using Core.Other;
using Core.Player;
using Core.UI;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.Infrastructure
{
    public class GameLostState : State
    {
        private readonly HeroSkins _heroSkins;
        private readonly IPlaythroughProgressHandler _playthrough;
        private readonly UIProvider _uiProvider;
        private readonly IAudioHandler _audioHandler;
        private GameLostMenuView _menuView;

        [Inject]
        public GameLostState(
            HeroSkins heroSkins,
            IPlaythroughProgressHandler playthrough,
            UIProvider uiProvider, 
            IAudioHandler audioHandler)
        {
            _heroSkins = heroSkins;
            _playthrough = playthrough;
            _uiProvider = uiProvider;
            _audioHandler = audioHandler;
        }

        public override void Enter()
        {
            _audioHandler.PlaySound(AudioName.GameLost);
            
            if (_menuView == null)
            {
                _menuView = _uiProvider.CreateGameLostMenu();
                _menuView.Initialize(new GameLostMenuPresenter(
                    _heroSkins,
                    _playthrough,
                    _menuView));
            }

            _menuView.Reveal(enable: true).Forget();
        }

        public override void Exit() => 
            _menuView.SetActive(false);
    }
}