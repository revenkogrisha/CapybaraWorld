using Core.Saving;
using Zenject;

namespace Core.Player
{
    public class PlayerUpgrade : ISaveable
    {
        private const int StartHeroLevel = 1;
        private const int CostUpgradeIncrease = 15;
        private const int DefaultCost = 15;

        private readonly ISaveService _saveService;
        private readonly PlayerData _playerData;
        private readonly UpgradableStat[] _upgradableStats;
        private int _heroLevel = StartHeroLevel;
        private int _cost = 15;
        
        public UpgradableStat DashCooldownBonus { get; }
        public UpgradableStat DashSpeedBonus { get; }
        
        public int Cost => _cost;
        public int HeroLevel => _heroLevel;
        public bool CanUpgradeHero => _playerData.CoinsAmount >= _cost;
        public bool IsCostIncreased => DefaultCost < _cost;

        [Inject]
        public PlayerUpgrade(
            ISaveService saveService, 
            PlayerData playerData, 
            PlayerUpgradeConfig config)
        {
            _saveService = saveService;
            _playerData = playerData;

            DashCooldownBonus = new(config.DashCooldownStep, config.DashCooldownMax);
            DashSpeedBonus = new(config.DashSpeedStep, config.DashSpeedMax);
            
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

            RestoreStats();
        }

        public void ResetCost()
        {
            _cost = DefaultCost;
            _saveService.Save();
        }

        public void UpgradeHero(bool force = false)
        {
            if (force == false)
            {
                _playerData.RemoveCoins(_cost);
                _cost += CostUpgradeIncrease;
            }
            
            _heroLevel++;

            _saveService.Save();

            UpgradeAllStats();
        }

        private void RestoreStats()
        {
            for (int i = 0; i < _heroLevel - StartHeroLevel; i++)
                UpgradeAllStats();
        }

        private void UpgradeAllStats()
        {
            foreach (UpgradableStat stat in _upgradableStats) 
                stat.Upgrade();
        }
    }
}