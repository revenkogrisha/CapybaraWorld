using Core.Common;
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

        [Inject]
        public DataInitializationState(
            ILocationsHandler locationsHandler,
            ISaveService saveService,
            GameNavigation navigation,
            PlayerData data)
        {
            _locationsHandler = locationsHandler;
            _saveService = saveService;
            _navigation = navigation;
            _data = data;
        }
        
        public override void Enter()
        {
            _saveService.Register(_locationsHandler);
            _saveService.Register(_data);
            _saveService.Load();

            _navigation.Generate<MainMenuState>();
        }
    }
}