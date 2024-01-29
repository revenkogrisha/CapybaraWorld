using Core.Editor.Debugger;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Core.Mediation.UnityAds
{
    public class UnityAdsService : IMediationService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private bool _isRewardedAvailable = false;
        private float _nextAdShow;

        public bool IsRewardedAvailable => _isRewardedAvailable;
        public bool CanShow => Time.time > _nextAdShow;

        public void Initialize()
        {
            UnityAdsInitializer.Initialize(this);
            _nextAdShow = UnityAdsData.AdShowStartupDelay;

            LoadInterstitial();
        }

        public void ShowInterstitial()
        {
            if (CanShow == false)
                return;
            
#if UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Show(UnityAdsData.AndroidInterstitialId, this);
#else
            Advertisement.Show(UnityAdsData.IOSInterstitialId, this);
#endif
        }

        public void ShowInterstitialForce()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Show(UnityAdsData.AndroidInterstitialId, this);
#else
            Advertisement.Show(UnityAdsData.IOSInterstitialId, this);
#endif
        }

        public void ShowRewarded()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Show(UnityAdsData.AndroidRewardedId, this);
#else
            Advertisement.Show(UnityAdsData.IOSRewardedId, this);
#endif
        }
        
        #region Callbacks

        public void OnInitializationComplete() => 
            RDebug.Info($"Unity Ads successfully initialized!");

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) => 
            RDebug.Error($"Unity Ads initialization failed: E: {error} \n M: {message}!");

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) => 
            RDebug.Error($"Unity Ads load failed: E: {error} \n M: {message}!");

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) => 
            RDebug.Error($"Unity Ads show failed: E: {error} \n M: {message}!");

        public void OnUnityAdsAdLoaded(string placementId) {  }

        public void OnUnityAdsShowStart(string placementId) {  }

        public void OnUnityAdsShowClick(string placementId) {  }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) 
        {
            if (IsInterstitial(placementId) == true)
            {
                _nextAdShow += UnityAdsData.AdShowInterval;
                RDebug.Info($"{nameof(UnityAdsService)}: Interstitial showed!");
                
                LoadInterstitial();
            }
            else if (IsRewarded(placementId) == true)
            {
                RDebug.Info($"{nameof(UnityAdsService)}: Rewarded showed!");
            }
        }

        #endregion

        private bool IsInterstitial(string placementId)
        {
            return placementId.Equals(UnityAdsData.AndroidInterstitialId) == true
                || placementId.Equals(UnityAdsData.IOSInterstitialId) == true;
        }

        private bool IsRewarded(string placementId)
        {
            return placementId.Equals(UnityAdsData.AndroidRewardedId) == true
                || placementId.Equals(UnityAdsData.IOSRewardedId) == true;
        }

        private void LoadInterstitial()
        {
#if UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Load(UnityAdsData.AndroidRewardedId, this);
#else
            Advertisement.Load(UnityAdsData.IOSRewardedId, this);
#endif
        }
    }
}