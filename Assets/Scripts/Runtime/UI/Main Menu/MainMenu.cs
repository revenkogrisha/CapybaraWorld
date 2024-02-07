using Core.Infrastructure;
using Core.Player;
using Core.Level;
using Zenject;

namespace Core.UI
{
    public class MainMenu
    {
        private ILocationsHandler _locationsHandler;
        private PlayerData _playerData;
        private GameNavigation _navigation;
        
        [Inject]
        public MainMenu(
            ILocationsHandler locationsHandler,
            PlayerData playerData,
            GameNavigation navigation)
        {
            _locationsHandler = locationsHandler;
            _playerData = playerData;
            _navigation = navigation;
        }

        public void StartGame() => 
            _navigation.ToGameplay();

        public string GetLocationName() => 
            _locationsHandler.CurrentLocation.Name;

        public int GetLevelNumber() => 
            _playerData.LevelNumber;
    }
}