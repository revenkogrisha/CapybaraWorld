using System.Threading;
using Core.Editor.Debugger;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Core.UI
{
    public class MainMenuView : AnimatedUI
    {
        private const string LevelTextFormat = "Level {0}";
        private const string LocationTextFormat = "Location: <b><color=#D978E9>{0}</color></b>";
        
        [Space]
        [SerializeField] private UIButton _playButton;
        [SerializeField] private UIButton _upgradeButton;
        
        [Space]
        [SerializeField] private TMP_Text _levelTMP;
        [SerializeField] private TMP_Text _locationTMP;

        [Header("Dev Buttons")] 
        [SerializeField] private UIButton _devLocationButton;
        [SerializeField] private UIButton _devLevelButton;
        [SerializeField] private UIButton _devNotificationButton;
        [SerializeField] private UIButton _devShowAdButton;

        private MainMenuRoot _root;
        private MainMenuPresenter _presenter;

        #region MonoBehaviour

        private void OnEnable()
        {
            _playButton.OnClicked += StartGame;
            _upgradeButton.OnClicked += ToHeroMenu;

#if REVENKO_DEVELOP
            _devLocationButton.OnClicked += DevUpdateLocation;
            _devLevelButton.OnClicked += DevCompleteLevel;
            _devShowAdButton.OnClicked += DevShowAd;
    #if UNITY_ANDROID && !UNITY_EDITOR
            _devNotificationButton.OnClicked += DevSendNotification;
    #endif
#endif
        }

        private void OnDisable()
        {
            _playButton.OnClicked -= StartGame;
            _upgradeButton.OnClicked -= ToHeroMenu;

#if REVENKO_DEVELOP
            _devLocationButton.OnClicked -= DevUpdateLocation;
            _devLevelButton.OnClicked -= DevCompleteLevel;
            _devShowAdButton.OnClicked -= DevShowAd;
    #if UNITY_ANDROID && !UNITY_EDITOR
            _devNotificationButton.OnClicked -= DevSendNotification;
    #endif
#endif
        }

        #endregion

        public override UniTask Reveal(CancellationToken token = default, bool enable = false)
        {
            DisplayLevelNumber();
            DisplayLocationName();

            return base.Reveal(token, enable);
        }

        public void Initialize(MainMenuRoot root, MainMenuPresenter presenter)
        {
            _root = root;
            _presenter = presenter;
        }

        private void StartGame() => 
            _presenter.OnStartGame();

        private void ToHeroMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowHeroMenu();
        }

        private void DisplayLevelNumber()
        {
            string levelText = string.Format(LevelTextFormat, _presenter.GetLevelNumber());
            _levelTMP.SetText(levelText);
        }

        private void DisplayLocationName()
        {
            string locationName = _presenter.GetLocationName();
            string locationText = string.Format(LocationTextFormat, locationName);
            _locationTMP.SetText(locationText);
        }

#if REVENKO_DEVELOP
        private void DevUpdateLocation()
        {
            _presenter.OnDevUpdateLocation();
            
            DisplayLevelNumber();
            DisplayLocationName();
            RDebug.Log($"{nameof(MainMenuView)}: Location updated");
        }

        private void DevCompleteLevel() => 
            _presenter.OnDevCompleteLevel();

        private void DevShowAd() => 
            _presenter.OnDevShowAd();

    #if UNITY_ANDROID && !UNITY_EDITOR
        private void DevSendNotification() =>
            _presenter.OnDevSendNotification();
    #endif
#endif
    }
}