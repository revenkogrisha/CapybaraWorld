using YG;

namespace Core.Mediation
{
    public class YGService : IMediationService
    {
        public bool IsRewardedAvailable => IsEnabled;
        public bool IsEnabled => YandexGame.SDKEnabled;

        public void Initialize() {  }

        public void LoadRewarded() {  }

        public bool ShowInterstitial()
        {
            if (IsEnabled == true)
                return YandexGame.FullscreenShow();
            
            return false;
        }

        public void ShowInterstitialForce() => 
            ShowInterstitial();

        public void ShowRewarded(IAdRewardWaiter waiter)
        {
            if (IsEnabled == true)
                YandexGame.RewVideoShow(waiter.RewardID);
        }
    }
}