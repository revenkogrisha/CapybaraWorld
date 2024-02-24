using System;
using Core.Audio;
using Core.Infrastructure;
using Core.Mediation;
using Core.Other;
using DG.Tweening;
using TriInspector;
using UnityEngine;
using YG;
using Zenject;

namespace Core.UI
{
    public class FinishMenu : AnimatedUI
    {
        private const float TitleTweenDuration = 0.5f;
        private const float ButtonShakeDelayByTitle = 0.5f;
        
        [Title("Buttons")]
        [SerializeField] private Transform _title;
        [SerializeField] private bool _tween = true;
        [SerializeField, ShowIf(nameof(_tween))] private float _finalScale = 1f;
        
        [Title("Buttons")]
        [SerializeField] private UIButton _restartButton;
        [SerializeField] private UIButton _menuButton;

        private GameNavigation _navigation;
        private IMediationService _mediationService;
        private IAudioHandler _audioHandler;

        protected virtual void OnEnable()
        {
            _restartButton.OnClicked += RestartGame;
            _menuButton.OnClicked += ReturnToMenu;

            YandexGame.OpenFullAdEvent += TurnOffSounds;
            YandexGame.CloseFullAdEvent += TurnOnSounds;
            YandexGame.ErrorFullAdEvent += TurnOnSounds;

            TweenElements();
        }

        protected virtual void OnDisable()
        {
            _restartButton.OnClicked -= RestartGame;
            _menuButton.OnClicked -= ReturnToMenu;
            
            YandexGame.OpenFullAdEvent -= TurnOffSounds;
            YandexGame.CloseFullAdEvent -= TurnOnSounds;
            YandexGame.ErrorFullAdEvent -= TurnOnSounds;
        }

        [Inject]
        private void BaseConstruct(
            GameNavigation navigation,
            IMediationService mediationService,
            IAudioHandler audioHandler) 
        {
            _navigation = navigation;
            _mediationService = mediationService;
            _audioHandler = audioHandler;
        }

        private void TurnOnSounds()
        {
            print("2Unmute");
            if (_audioHandler is UnityAudioHandler unityAudio)
                unityAudio.Unmute();
        }

        private void TurnOffSounds()
        {
            print("2Mute");
            if (_audioHandler is UnityAudioHandler unityAudio)
                unityAudio.Mute();
        }

        private void RestartGame()
        {
            bool shown = _mediationService.ShowInterstitial();
            if (shown == true)
                return;
            
            _navigation.Generate<GameplayState>();
        }

        private void ReturnToMenu() => 
            _navigation.Generate<MainMenuState>();

        private void TweenElements()
        {
            if (_tween == true)
            {
                _title.localScale = Vector2.zero;
                DOTween.Sequence()
                    .Append(_title.DOScale(_finalScale, TitleTweenDuration))
                    .AppendInterval(ButtonShakeDelayByTitle)
                    .AppendCallback(() => _restartButton.Shake());
            }
        }
    }
}