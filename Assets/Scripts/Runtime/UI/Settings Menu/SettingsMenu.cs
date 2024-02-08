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
        private const float VolumeOnValue = 0f;
        private const float VolumeOffValue = -80f;
        
        private readonly IAudioHandler _audioHandler;
#pragma warning disable IDE0052 // Remove unread private members
        private readonly ISaveService _saveService;
#pragma warning restore IDE0052
        private bool _isMusicOn = true;
        private bool _areSoundsOn = true;

        public bool IsMusicOn => _isMusicOn;
        public bool AreSoundsOn => _areSoundsOn;

        [Inject]
        public SettingsMenu(IAudioHandler audioHandler, ISaveService saveService)
        {
            _audioHandler = audioHandler;
            _saveService = saveService;
        }

        public void InitializeOnDataLoaded(float musicVolume, float soundsVolume)
        {
            _isMusicOn = musicVolume >= VolumeOnValue;
            _areSoundsOn = soundsVolume >= VolumeOnValue;
        }

        public bool ToggleMusic()
        {
            _isMusicOn = !_isMusicOn;

            _audioHandler.SetMusicVolume(_isMusicOn == true ? VolumeOnValue : VolumeOffValue);
            
            return _isMusicOn;
        }

        public bool ToggleSounds()
        {
            _areSoundsOn = !_areSoundsOn;

            _audioHandler.SetSoundsVolume(_areSoundsOn == true ? VolumeOnValue : VolumeOffValue);
            
            return _areSoundsOn;
        }

        public void TryLoadProgressOrSignIn()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            SignInService.Authenticate().Forget();
#endif
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
