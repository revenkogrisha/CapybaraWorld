#if REVENKO_DEVELOP
using Core.Editor.Debugger;
#endif
using Core.Game;
using Core.Infrastructure;
using Core.Level;
using Core.Player;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class MainMenu : AnimatedUI
    {
        private const string LevelTextFormat = "Level {0}";
        private const string LocationTextFormat = "Location: {0}";
        
        [Space]
        [SerializeField] private UIButton _playButton;
        [SerializeField] private UIButton _upgradeButton;
        
        [Space]
        [SerializeField] private TMP_Text _levelTMP;
        [SerializeField] private TMP_Text _locationTMP;

        [Header("Dev Buttons")] 
        [SerializeField] private UIButton _devLocationButton;
        [SerializeField] private UIButton _devLevelButton;

#if REVENKO_DEVELOP
        private PlaythroughHandler _playthroughHandler;
#endif
        private ILocationsHandler _locationsHandler;
        private PlayerData _playerData;
        private GameNavigation _navigation;
        private MainMenuRoot _root;

        #region MonoBehaviour

        private void OnEnable()
        {
            _playButton.OnClicked += StartGame;
            _upgradeButton.OnClicked += ToUpgradeMenu;

#if REVENKO_DEVELOP
            _devLocationButton.OnClicked += UpdateLocation;
            _devLevelButton.OnClicked += CompleteLevel;
#endif
        }

        private void OnDisable()
        {
            _playButton.OnClicked -= StartGame;
            _upgradeButton.OnClicked -= ToUpgradeMenu;

#if REVENKO_DEVELOP
            _devLocationButton.OnClicked -= UpdateLocation;
            _devLevelButton.OnClicked -= CompleteLevel;
#endif
        }

        private void Start()
        {
            DisplayLevelNumber();
            DisplayLocationName();
        }

        #endregion

        [Inject]
        private void Construct(
#if REVENKO_DEVELOP
            PlaythroughHandler playthroughHandler,
#endif
            ILocationsHandler locationsHandler,
            PlayerData playerData,
            GameNavigation navigation)
        {
#if REVENKO_DEVELOP
            _playthroughHandler = playthroughHandler;
#endif
            _locationsHandler = locationsHandler;
            _playerData = playerData;
            _navigation = navigation;
        }

        public void InitializeRoot(MainMenuRoot root) => 
            _root = root;

        private void StartGame() => 
            _navigation.ToGameplay();

        private void ToUpgradeMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowUpgradeMenu();
        }

        private void DisplayLevelNumber()
        {
            string levelText = string.Format(LevelTextFormat, _playerData.LevelNumber);
            _levelTMP.SetText(levelText);
        }

        private void DisplayLocationName()
        {
            string locationName = _locationsHandler.CurrentLocation.Name;
            string locationText = string.Format(LocationTextFormat, locationName);
            _locationTMP.SetText(locationText);
        }

#if REVENKO_DEVELOP
        private void UpdateLocation()
        {
            _locationsHandler.UpdateLocation();
            
            DisplayLevelNumber();
            DisplayLocationName();
            RDebug.Log($"{nameof(MainMenu)}: Location updated");
            
            _navigation.Generate<MainMenuState>();
            RDebug.Log($"{nameof(MainMenu)}: Regenerated!");
        }

        private void CompleteLevel()
        {
            _navigation.ToGameplay();
            _playthroughHandler.FinishGame(GameResult.Win);
            RDebug.Log($"{nameof(MainMenu)}: Level completed");
        }
#endif
    }
}