using System.Threading;
using Core.Audio;
using Core.Mediation;
using Core.Other;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using YG;
using Zenject;

namespace Core.UI
{
    public class MainMenuView : AnimatedUI
    {
        private const string LevelTextFormat = "Уровень {0}";
        private const string LocationTextFormat = "Локация <b><color=#D978E9>{0}</color></b>";
        
        [Header("Menu Buttons")] 
        [SerializeField] private UIButton _playButton;
        [SerializeField] private UIButton _upgradeButton;
        [SerializeField] private UIButton _settingsButton;
        
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
        private IMediationService _mediationService;
        private IAudioHandler _audioHandler;
        private bool _triedShowAd = false;

        #region MonoBehaviour

        private void OnEnable()
        {
            _playButton.OnClicked += StartGame;
            _upgradeButton.OnClicked += ToHeroMenu;
            _settingsButton.OnClicked += ToSettingsMenu;

            YandexGame.OpenFullAdEvent += TurnOffSounds;
            YandexGame.CloseFullAdEvent += TurnOnSounds;
            YandexGame.ErrorFullAdEvent += TurnOnSounds;

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
            _settingsButton.OnClicked -= ToSettingsMenu;

            YandexGame.OpenFullAdEvent -= TurnOffSounds;
            YandexGame.CloseFullAdEvent -= TurnOnSounds;
            YandexGame.ErrorFullAdEvent -= TurnOnSounds;

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

            _triedShowAd = false;

            return base.Reveal(token, enable);
        }

        [Inject]
        private void Construct(IMediationService mediationService, IAudioHandler audioHandler)
        {
            _mediationService = mediationService;
            _audioHandler = audioHandler;
        }

        public void Initialize(MainMenuRoot root, MainMenuPresenter presenter)
        {
            _root = root;
            _presenter = presenter;
        }

        private void StartGame()
        {
            bool shown = _mediationService.ShowInterstitial();

            if (shown == true)
                return;
            
            _presenter.OnStartGame();
        }

        private void TurnOnSounds()
        {
            print("1Unmute");
            if (_audioHandler is UnityAudioHandler unityAudio)
                unityAudio.Unmute();
        }

        private void TurnOffSounds()
        {
            print("1Mute");
            if (_audioHandler is UnityAudioHandler unityAudio)
                unityAudio.Mute();
        }

        private void ToHeroMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowHeroMenu();
        }

        private void ToSettingsMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowSettingsMenu();
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