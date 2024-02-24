using Core.Audio;
using Core.Common;
using Core.Level;
using Core.Player;
using Core.Saving;
using Core.UI;
using UnityEngine;
using YG;
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
        private readonly IAudioHandler _audioHandler;
        private readonly SettingsMenu _settingsMenu;

        [Inject]
        public DataInitializationState(
            HeroSkins heroSkins,
            ILocationsHandler locationsHandler,
            ISaveService saveService,
            GameNavigation navigation,
            PlayerData data, 
            PlayerUpgrade upgrade,
            IAudioHandler audioHandler,
            SettingsMenu settingsMenu)
        {
            _heroSkins = heroSkins;
            _locationsHandler = locationsHandler;
            _saveService = saveService;
            _navigation = navigation;
            _data = data;
            _upgrade = upgrade;
            _audioHandler = audioHandler;
            _settingsMenu = settingsMenu;
        }
        
        public override void Enter()
        {
            RegisterSaveables();
            _saveService.Load();

            _audioHandler.Initialize();

            AssingFromPlayerPrefs();

            _navigation.Generate<MainMenuState>();

            YandexGame.GameReadyAPI();
            Debug.Log("<YG> Game Ready API");
        }

        private void RegisterSaveables()
        {
            _saveService.Register(_locationsHandler);
            _saveService.Register(_data);
            _saveService.Register(_upgrade);
            _saveService.Register(_heroSkins);
        }

        private void AssingFromPlayerPrefs()
        {
            _settingsMenu.InitializeOnDataLoaded(
                PlayerPrefsUtility.IsMusicOn, 
                PlayerPrefsUtility.AreSoundsOn);
        }
    }
}