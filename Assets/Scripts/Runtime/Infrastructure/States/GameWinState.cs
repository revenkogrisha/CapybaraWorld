using System.Threading;
using Core.Audio;
using Core.Common;
using Core.Common.ThirdParty;
using Core.Editor.Debugger;
using Core.Factories;
using Core.Other;
using Core.Saving;
using Core.UI;
using Cysharp.Threading.Tasks;
using Zenject;

namespace Core.Infrastructure
{
    public class GameWinState : State
    {
        private const int LevelsWonToRequestReview = 3;
        
        private readonly UIProvider _uiProvider;
        private readonly IAudioHandler _audioHandler;
        private GameWinMenu _menuView;
        private CancellationTokenSource _cts;

        [Inject]
        public GameWinState(UIProvider uiProvider, IAudioHandler audioHandler)
        {
            _uiProvider = uiProvider;
            _audioHandler = audioHandler;
        }

        public override void Enter()
        {
            _audioHandler.PlaySound(AudioName.GameWin);
            
            if (_menuView == null)
                _menuView = _uiProvider.CreateGameWinMenu();

            _menuView.Reveal(enable: true).Forget();
            
            if (PlayerPrefsUtility.LevelsWon >= LevelsWonToRequestReview 
                && PlayerPrefsUtility.HasRequestedReview == false)
                RequestUserReview();
        }

        public override void Exit()
        {
            _menuView.SetActive(false);
            _cts.Clear();
        }

        private void RequestUserReview()
        {
            const float reviewTimeout = 120f;
        
            IUserReviewService reviewService = new YGReviewService();
            
            if (reviewService.IsFake == true)
                RDebug.Info($"User review request will be faked");
            
            _cts = new();
            _cts.CancelByTimeout(reviewTimeout).Forget();

            reviewService.Initialize();
            reviewService.Request(_cts.Token).Forget();

            PlayerPrefsUtility.HasRequestedReview = true;
        }
    }
}