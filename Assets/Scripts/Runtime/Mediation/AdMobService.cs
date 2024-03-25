using Core.Editor.Debugger;
using GoogleMobileAds.Api;

namespace Core.Mediation
{
    public class AdMobService : IMediationService
    {
        private const string InterstitialTestUnit = "ca-app-pub-3940256099942544/1033173712";
        private const string RewardedTestUnit = "ca-app-pub-3940256099942544/5224354917";
        
        private InterstitialAd _interstitialAd = null;
        private RewardedAd _rewardedAd = null;

        public bool IsRewardedAvailable => _rewardedAd != null && _rewardedAd.CanShowAd() == true;

        public void Initialize() => 
            MobileAds.Initialize(OnInitializationComplete);

        public void LoadRewarded()
        {
            const string log = nameof(AdMobService) + "::" + nameof(LoadRewarded) + ":";

            if (_rewardedAd != null)
            {
                _rewardedAd.Destroy();
                _rewardedAd = null;
            }

            RDebug.Log($"{log} Loading the rewarded");

            AdRequest adRequest = new();
            RewardedAd.Load(RewardedTestUnit, adRequest, OnAdLoading);
        }

        public void ShowInterstitial()
        {
            const string log = nameof(AdMobService) + "::" + nameof(ShowInterstitial) + ":";
            
            if (_interstitialAd != null && _interstitialAd.CanShowAd() == true)
            {
                RDebug.Log($"{log} Showing interstitial ad");
                _interstitialAd.Show();

                RegisterEventsHandler(_interstitialAd);
            }
            else
            {
                RDebug.Error($"{log} Interstitial ad is not ready yet.");
            }
        }

        public void ShowInterstitialForce() => 
            ShowInterstitial();

        public void ShowRewarded(IAdRewardWaiter waiter)
        {
            if (IsRewardedAvailable == true)
            {
                _rewardedAd.Show(_ =>
                {
                    waiter.OnRewardGranted();
                    waiter = null;
                });
            }
        }

        private void LoadInterstitial()
        {
            const string log = nameof(AdMobService) + "::" + nameof(LoadInterstitial) + ":";

            if (_interstitialAd != null)
            {
                _interstitialAd.Destroy();
                _interstitialAd = null;
            }

            RDebug.Log($"{log} Loading the interstitial");

            AdRequest adRequest = new();
            InterstitialAd.Load(InterstitialTestUnit, adRequest, OnAdLoading);
        }

        private void RegisterEventsHandler(InterstitialAd interstitialAd) => 
            interstitialAd.OnAdFullScreenContentClosed += OnInterstitialEnded;

        private void UnRegisterEventsHandler(InterstitialAd interstitialAd) => 
            interstitialAd.OnAdFullScreenContentClosed -= OnInterstitialEnded;

        #region Callbacks

        private void OnInitializationComplete(InitializationStatus status)
        {
            RDebug.Log($"AdMob initialization complete. Status: {status}");

            LoadInterstitial();
        }

        private void OnAdLoading(InterstitialAd ad, LoadAdError error)
        {
            const string log = nameof(AdMobService) + "::" + nameof(OnAdLoading) + ":";
        
            if (error != null || ad == null)
            {
                RDebug.Error($"{log} Interstitial failed to load. Error: {error}");
                return;
            }

            RDebug.Log($"{log} Interstitial loaded with response: " + ad.GetResponseInfo());

            _interstitialAd = ad;
        }

        private void OnAdLoading(RewardedAd ad, LoadAdError error)
        {
            const string log = nameof(AdMobService) + "::" + nameof(OnAdLoading) + ":";
        
            if (error != null || ad == null)
            {
                RDebug.Error($"{log} Rewarded failed to load. Error: {error}");
                return;
            }

            RDebug.Log($"{log} Rewarded loaded with response: " + ad.GetResponseInfo());

            _rewardedAd = ad;
        }

        private void OnInterstitialEnded()
        {
            UnRegisterEventsHandler(_interstitialAd);

            _interstitialAd.Destroy();
            _interstitialAd = null;

            LoadInterstitial();
        }

        #endregion
    }
}