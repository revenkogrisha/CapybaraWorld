using System;
using System.Threading;
using Core.Editor.Debugger;
using Core.Other;
using Core.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Player
{
    public class DashRecoveryDisplay : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Slider _slider;
        [SerializeField] private AnimatedUI _animatedUI;
        [SerializeField] private Canvas _canvas;

        [Header("Configuration")]
        [SerializeField] private HeroConfig _config;
        [SerializeField] private Vector2 _positionOffset = new(0f, 1f);

        private readonly CompositeDisposable _disposable = new();
        private Hero _hero;
        private CancellationToken _cancellationToken;
        private Transform _thisTransform;
        private Transform _heroTransform;
        private bool _displaying = false;

        #region MonoBehaviour

        private void OnDestroy() => 
            _disposable.Clear();

        #endregion

        public void Initialize(Hero hero)
        {
            _hero = hero;
            _thisTransform = transform;
            _heroTransform = _hero.transform;

            _canvas.worldCamera = Camera.main;

            _cancellationToken = destroyCancellationToken;
            
            Subscribe();
        }

        private void Subscribe()
        {
            _hero.DashedCommand
                .Where(_ => _displaying == false)
                .Subscribe(_ => Display(_cancellationToken).Forget())
                .AddTo(_disposable);

            this.UpdateAsObservable()
                .Where(_ => _displaying == true)
                .Subscribe(_ => FollowHero())
                .AddTo(_disposable);
        }

        private async UniTaskVoid Display(CancellationToken token)
        {
            try
            {
                _animatedUI.Reveal(token).Forget();
                _displaying = true;

                await UniTaskUtility.Delay(_config.DashDuration, token);

                await AnimateCooldown(token);

                await _animatedUI.Conceal(token);
                _displaying = false;
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(DashRecoveryDisplay)}::{nameof(Display)}: {ex.Message} \n{ex.StackTrace}");
            }
        }

        private void FollowHero()
        {
            Vector2 position = (Vector2)_heroTransform.position + _positionOffset;
            _thisTransform.position = position;
        }

        private async UniTask AnimateCooldown(CancellationToken token)
        {
            try
            {
                float duration = _config.DashCooldown;

                float elapsedTime = 0f;
                while (elapsedTime < duration)
                {
                    float delta = elapsedTime / duration;
                    _slider.value = delta;

                    elapsedTime += Time.deltaTime;

                    await UniTask.NextFrame(token);
                }
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(DashRecoveryDisplay)}::{nameof(AnimateCooldown)}: {ex.Message} \n{ex.StackTrace}");
            }
        }
    }
}