using Core.Infrastructure;
using Core.Level;
using Core.Player;
using TMPro;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class MainMenu : MonoBehaviour
    {
        private const string LevelTextFormat = "Level {0}";
        private const string LocationTextFormat = "Location: {0}";
        
        [SerializeField] private UIButton _playButton;
        [SerializeField] private TMP_Text _levelTMP;
        [SerializeField] private TMP_Text _locationTMP;

        private ILocationsHandler _locationsHandler;
        private PlayerData _playerData;
        private GameNavigation _navigation;

        #region MonoBehaviour

        private void OnEnable() => 
            _playButton.OnClicked += StartGame;

        private void OnDisable() =>
            _playButton.OnClicked -= StartGame;

        private void Start()
        {
            DisplayLevelNumber();
            DisplayLocationName();
        }

        #endregion

        [Inject]
        private void Construct(
            ILocationsHandler locationsHandler,
            PlayerData playerData,
            GameNavigation navigation)
        {
            _locationsHandler = locationsHandler;
            _playerData = playerData;
            _navigation = navigation;
        }

        private void StartGame() => 
            _navigation.ToGameplay();

        private void DisplayLevelNumber()
        {
            string levelText = string.Format(LevelTextFormat, _playerData.LevelNumber);
            _levelTMP.SetText(levelText);
        }

        private void DisplayLocationName()
        {
            string name = _locationsHandler.CurrentLocation.Name;
            string locationText = string.Format(LocationTextFormat, name);
            _locationTMP.SetText(locationText);
        }
    }
}