#if REVENKO_DEVELOP
using Core.Game;
using Core.Mediation;
#if UNITY_ANDROID && !UNITY_EDITOR
    using Core.Common.GameNotification;
#endif
using Core.Editor.Debugger;
using Zenject;
using Core.Level;
using Core.Player;
using Core.Infrastructure;

namespace Core.UI
{
    public class MainMenuDevHandler
    {
        private IMediationService _mediationService;
        private PlaythroughHandler _playthroughHandler;
        private ILocationsHandler _locationsHandler;
        private PlayerData _playerData;
        private GameNavigation _navigation;
#if UNITY_ANDROID && !UNITY_EDITOR
        private Notifications _notifications;
#endif

        [Inject]
        public MainMenuDevHandler(
            IMediationService mediationService,
            PlaythroughHandler playthroughHandler,
    #if UNITY_ANDROID && !UNITY_EDITOR
            Notifications notifications,
    #endif
            ILocationsHandler locationsHandler,
            PlayerData playerData,
            GameNavigation navigation)
        {
            _mediationService = mediationService;
            _playthroughHandler = playthroughHandler;
            _locationsHandler = locationsHandler;
            _playerData = playerData;
            _navigation = navigation;
    #if UNITY_ANDROID && !UNITY_EDITOR
            _notifications = notifications;
    #endif
        }
        
        public void UpdateLocation()
        {
            _locationsHandler.UpdateLocation();
            
            _navigation.Generate<MainMenuState>();
            RDebug.Log($"{nameof(MainMenuView)}: Regenerated!");
        }

        public void CompleteLevel()
        {
            _navigation.ToGameplay();
            _playthroughHandler.FinishGame(GameResult.Win);
            RDebug.Log($"{nameof(MainMenuView)}: Level completed");
        }

        public void ShowAd()
        {
            RDebug.Log($"{nameof(MainMenuView)}: Tried to show ad!");
            _mediationService.ShowInterstitialForce();
        }

    #if UNITY_ANDROID && !UNITY_EDITOR
        public void SendNotification()
        {
            _notifications.Send(_notifications.Collection.Test);
            _notifications.Send(_notifications.Collection.DelayedTest);
        }
    #endif
    }
}
#endif