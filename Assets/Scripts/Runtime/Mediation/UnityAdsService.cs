using Core.Editor.Debugger;
using UnityEngine;
using UnityEngine.Advertisements;

namespace Core.Mediation.UnityAds
{
    public class UnityAdsService : IMediationService, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        private const int LoadAtemptsRowMax = 3;
        
        private bool _isRewardedAvailable = false;
        private float _nextAdShow;
        private int _loadAttemptsRow = 0;
        private IAdRewardWaiter _rewardWaiter;

        public bool IsRewardedAvailable => _isRewardedAvailable;
        public bool CanShow => Advertisement.isInitialized && Time.time > _nextAdShow;

        public void Initialize()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
                return;
                
            UnityAdsInitializer.Initialize(this);
            _nextAdShow = Time.time + UnityAdsData.AdShowStartupDelay;

            _isRewardedAvailable = false;
            _rewardWaiter = null;
            _loadAttemptsRow = 0;
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

        public void ShowRewarded(IAdRewardWaiter waiter)
        {
            _rewardWaiter = waiter;
            
#if UNITY_ANDROID || UNITY_EDITOR
            Advertisement.Show(UnityAdsData.AndroidRewardedId, this);
#else
            Advertisement.Show(UnityAdsData.IOSRewardedId, this);
#endif

            _isRewardedAvailable = false;
        }

        public void LoadRewarded()
        {
            _isRewardedAvailable = false;

#if UNITY_ANDROID || UNITY_EDITOR
            RDebug.Log($"{nameof(UnityAdsService)}: Android {nameof(UnityAdsData.AndroidRewardedId)} load attempt");
            Advertisement.Load(UnityAdsData.AndroidRewardedId, this);
#else
            RDebug.Log($"{nameof(UnityAdsService)}: {nameof(UnityAdsData.IOSRewardedId)} load attempt");
            Advertisement.Load(UnityAdsData.IOSRewardedId, this);
#endif
        }

        #region Callbacks

        public void OnInitializationComplete()
        {
            RDebug.Info($"{nameof(UnityAdsService)}: successfully initialized!");
            LoadInterstitial();
        }

        public void OnInitializationFailed(UnityAdsInitializationError error, string message) => 
            RDebug.Error($"{nameof(UnityAdsService)}: initialization failed: E: {error} \n M: {message}!");

        public void OnUnityAdsAdLoaded(string placementId)
        {
            RDebug.Log($"{nameof(UnityAdsService)}: Ad was loaded: PID: {placementId}!");
            if (IsRewarded(placementId) == true)
                _isRewardedAvailable = true;
        }

        public void OnUnityAdsShowStart(string placementId) {  }

        public void OnUnityAdsShowClick(string placementId) {  }

        public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
        {
            RDebug.Error($"{nameof(UnityAdsService)}: load failed: E: {error} \n M: {message}!");

            if (IsInterstitial(placementId) == true)
                LoadInterstitial();
            else if (IsRewarded(placementId) == true)
                LoadRewarded();
        }

        public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
        {
            RDebug.Error($"{nameof(UnityAdsService)}: show failed: E: {error} \n M: {message}!");

            if (IsInterstitial(placementId) == true)
                LoadInterstitial();
            else if (IsRewarded(placementId) == true)
                LoadRewarded();
        }

        public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState) 
        {
            if (IsInterstitial(placementId) == true)
            {
                _nextAdShow = Time.time + UnityAdsData.AdShowInterval;
                
                LoadInterstitial();
            }
            else if (IsRewarded(placementId) == true)
            {
                if (_rewardWaiter != null)
                    _rewardWaiter.OnRewardGranted();
                else
                    RDebug.Error($"{nameof(UnityAdsService)}::{nameof(OnUnityAdsShowComplete)}: waiter is null!");
                    
                _rewardWaiter = null;
            }

            RDebug.Info($"{nameof(UnityAdsService)}: {placementId} ad showed!");
            _loadAttemptsRow = 0;
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
            if (_loadAttemptsRow >= LoadAtemptsRowMax)
            {
                RDebug.Log($"{nameof(UnityAdsService)}: Load returned. Too many attempts= {_loadAttemptsRow} >= maxValue= {LoadAtemptsRowMax}");
                return;
            }
                
            _loadAttemptsRow++;

#if UNITY_ANDROID || UNITY_EDITOR
            RDebug.Log($"{nameof(UnityAdsService)}: Android {nameof(UnityAdsData.AndroidInterstitialId)} load attempt");
            Advertisement.Load(UnityAdsData.AndroidInterstitialId, this);
#else
            RDebug.Log($"{nameof(UnityAdsService)}: {nameof(UnityAdsData.IOSInterstitialId)} load attempt");
            Advertisement.Load(UnityAdsData.IOSInterstitialId, this);
#endif
        }
    }
}