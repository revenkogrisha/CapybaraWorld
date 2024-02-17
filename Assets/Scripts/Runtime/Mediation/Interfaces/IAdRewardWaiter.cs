namespace Core.Mediation
{
    public interface IAdRewardWaiter
    {
        public int RewardID { get; }
        
        public void OnRewardGranted(int id);
    }
}
