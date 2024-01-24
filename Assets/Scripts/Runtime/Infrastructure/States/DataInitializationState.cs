using Core.Common;
using Core.Level;
using Core.Player;
using Core.Saving;
using Zenject;

namespace Core.Infrastructure
{
    public class DataInitializationState : State
    {
        private readonly HeroSkins _heroSkins;
        private readonly ILocationsHandler _locationsHandler;
        private readonly ISaveService _saveService;
        private readonly GameNavigation _navigation;
        private readonly PlayerData _data;
        private readonly PlayerUpgrade _upgrade;

        [Inject]
        public DataInitializationState(
            HeroSkins heroSkins,
            ILocationsHandler locationsHandler,
            ISaveService saveService,
            GameNavigation navigation,
            PlayerData data, 
            PlayerUpgrade upgrade)
        {
            _heroSkins = heroSkins;
            _locationsHandler = locationsHandler;
            _saveService = saveService;
            _navigation = navigation;
            _data = data;
            _upgrade = upgrade;
        }
        
        public override void Enter()
        {
            RegisterSaveables();
            _saveService.Load();

            _navigation.Generate<MainMenuState>();

            AssingFromPlayerPrefs();
        }

        private void RegisterSaveables()
        {
            _saveService.Register(_locationsHandler);
            _saveService.Register(_data);
            _saveService.Register(_upgrade);
            _saveService.Register(_heroSkins);
        }

        private void AssingFromPlayerPrefs() => 
            HapticHelper.Enabled = PlayerPrefsUtility.HapticFeedbackEnabled;
    }
}