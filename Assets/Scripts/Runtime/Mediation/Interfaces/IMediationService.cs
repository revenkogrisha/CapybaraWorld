namespace Core.Mediation
{
    public interface IMediationService
    {
        public bool IsRewardedAvailable { get; }
        
        public void Initialize();
        public bool ShowInterstitial(); 
        public void ShowInterstitialForce(); 
        public void ShowRewarded(IAdRewardWaiter waiter);
        public void LoadRewarded();
    }
}
