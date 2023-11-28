using Core.Common;
using Core.Player;
using Core.Saving;
using Zenject;

namespace Core.Infrastructure
{
    public class DataInitializationState : State
    {
        private readonly ISaveService _saveService;
        private readonly GameNavigation _navigation;
        private readonly PlayerData _data;

        [Inject]
        public DataInitializationState(
            ISaveService saveService,
            GameNavigation navigation,
            PlayerData data)
        {
            _saveService = saveService;
            _navigation = navigation;
            _data = data;
        }
        
        public override void Enter()
        {
            _saveService.Register(_data);
            _saveService.Load();

            _navigation.Generate<MainMenuState>();
        }
    }
}