using System.Threading;
using Core.Audio;
using Core.Common;
using Core.Common.ThirdParty;
using Core.Editor.Debugger;
using Core.Factories;
using Core.Other;
using Core.Saving;
using Core.UI;
using Zenject;

namespace Core.Infrastructure
{
    public class GameWinState : State
    {
        private const int LevelsWonToRequestReview = 3;
        
        private readonly UIProvider _uiProvider;
        private readonly IAudioHandler _audioHandler;
        private GameWinMenu _gameWinMenu;
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
            
            _gameWinMenu = _uiProvider.CreateGameWinMenu();
            
            if (PlayerPrefsUtility.LevelsWon >= LevelsWonToRequestReview 
                && PlayerPrefsUtility.HasRequestedReview == false)
                RequestUserReview();
        }

        public override void Exit()
        {
            _gameWinMenu.gameObject.SelfDestroy();
            _cts.Clear();
        }

        private void RequestUserReview()
        {
            const float reviewTimeout = 120f;
        
#if !UNITY_EDITOR && UNITY_ANDROID
            IUserReviewService reviewService = new GoogleUserReviewService();
#else
            IUserReviewService reviewService = new FakeUserReviewService();
#endif
            
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