using Core.Common;
using Core.Editor;
using Core.Level;
using Core.Player;
using Core.Saving;
using Zenject;

namespace Core.Infrastructure
{
    public class DataInitializationState : State
    {
        private readonly ILocationsHandler _locationsHandler;
        private readonly ISaveService _saveService;
        private readonly GameNavigation _navigation;
        private readonly PlayerData _data;
        private readonly PlayerUpgrade _upgrade;

        [Inject]
        public DataInitializationState(
            ILocationsHandler locationsHandler,
            ISaveService saveService,
            GameNavigation navigation,
            PlayerData data, 
            PlayerUpgrade upgrade)
        {
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
        }

        private void RegisterSaveables()
        {
            _saveService.Register(_locationsHandler);
            _saveService.Register(_data);
            _saveService.Register(_upgrade);
        }
    }
}