#if UNITY_ANDROID && !UNITY_EDITOR
using Core.Common.ThirdParty;
using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
#endif
using Core.Audio;
using Core.Saving;
using Zenject;

namespace Core.UI
{
    public class SettingsMenu
    {
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ISaveService _saveService;
#pragma warning restore IDE0052
        private readonly IAudioHandler _audioHandler;
        private AudioConfig _config;
        private bool _isMusicOn = true;
        private bool _areSoundsOn = true;

        public bool IsMusicOn => _isMusicOn;
        public bool AreSoundsOn => _areSoundsOn;

        [Inject]
        public SettingsMenu(
            AudioConfig config, 
            IAudioHandler audioHandler, 
            ISaveService saveService)
        {
            _config = config;
            _audioHandler = audioHandler;
            _saveService = saveService;
        }

        public void InitializeOnDataLoaded(bool isMusicOn, bool areSoundsOn)
        {
            _isMusicOn = isMusicOn;
            _areSoundsOn = areSoundsOn;

            AssignMusic();
            AssignSounds();
        }

        public bool ToggleMusic()
        {
            _isMusicOn = !_isMusicOn;
            
            AssignMusic();

            PlayerPrefsUtility.IsMusicOn = _isMusicOn;
            return _isMusicOn;
        }

        public bool ToggleSounds()
        {
            _areSoundsOn = !_areSoundsOn;
            
            AssignSounds();
            
            PlayerPrefsUtility.AreSoundsOn = _areSoundsOn;
            return _areSoundsOn;
        }

        public void TryLoadProgressOrSignIn()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            LoadProgressFromCloud();
#endif
        }

        private void AssignMusic()
        {
            _audioHandler.SetMusicVolume(_isMusicOn == true 
                ? _config.MusicVolume 
                : _config.VolumeOffValue);
        }

        private void AssignSounds()
        {
            _audioHandler.SetSoundsVolume(_areSoundsOn == true 
                ? _config.SoundsVolume 
                : _config.VolumeOffValue);
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private async void LoadProgressFromCloud()
        {
            await SignInService.Authenticate();
            _saveService.Load();
            Application.Quit();
        }
#endif
    }
}
