using Core.Saving;
using Zenject;

namespace Core.Player
{
    public class PlayerUpgrade : ISaveable
    {
        private const int CostUpgradeIncrease = 15;

        private const float DashCooldownMax = 2f;
        private const float DashSpeedMax = 1.5f;
        
        private const float DashCooldownUpgradeStep = 0.05f;
        private const float DashSpeedUpgradeStep = 0.05f;
        
        private readonly PlayerData _playerData;
        private readonly UpgradableStat[] _upgradableStats;
        private int _heroLevel = 1;
        private int _cost = 15;
        
        public UpgradableStat DashCooldownBonus { get; }
        public UpgradableStat DashSpeedBonus { get; }
        
        public int Cost => _cost;
        public int HeroLevel => _heroLevel;
        public bool CanUpgradeHero => _playerData.CoinsAmount > _cost;

        [Inject]
        public PlayerUpgrade(PlayerData playerData)
        {
            _playerData = playerData;

            DashCooldownBonus = new(DashCooldownUpgradeStep, DashCooldownMax);
            DashSpeedBonus = new(DashSpeedUpgradeStep, DashSpeedMax);
            
            _upgradableStats = new[]
            {
                DashCooldownBonus,
                DashSpeedBonus
            };
        }

        public void Save(SaveData data)
        {
            data.UpgradeCost = _cost;
            data.HeroLevel = _heroLevel;
        }

        public void Load(SaveData data)
        {
            _cost = data.UpgradeCost;
            _heroLevel = data.HeroLevel;
        }

        public void UpgradeHero()
        {
            _playerData.RemoveCoins(_cost);
            
            _heroLevel++;
            _cost += CostUpgradeIncrease;

            foreach (UpgradableStat stat in _upgradableStats) 
                stat.Upgrade();
        }
    }
}