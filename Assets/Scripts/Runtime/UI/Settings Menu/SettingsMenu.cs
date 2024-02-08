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
        private const float MusicOnValue = 0f;
        private const float MusicOffValue = -80f;
        
        private readonly IAudioHandler _audioHandler;
        private readonly ISaveService _saveService;
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

        public bool ToggleMusic()
        {
            _isMusicOn = !_isMusicOn;

            _audioHandler.SetMusicVolume(_isMusicOn == true ? MusicOnValue : MusicOffValue);
            
            return _isMusicOn;
        }

        public bool ToggleSounds()
        {
            _areSoundsOn = !_areSoundsOn;

            _audioHandler.SetSoundsVolume(_areSoundsOn == true ? MusicOnValue : MusicOffValue);
            
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
