using System.Threading;
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
        [SerializeField] private Slider _slider;
        [SerializeField] private AnimatedUI _animatedUI;
        [SerializeField] private HeroConfig _config;
        [SerializeField] private Canvas _canvas;

        [Space]
        [SerializeField] private Vector2 _positionOffset = new(0f, 1f);

        private readonly CompositeDisposable _disposable = new();
        private Hero _hero;
        private CancellationTokenSource _cts;
        private Transform _thisTransform;
        private Transform _heroTransform;
        private bool _displaying = false;

        #region MonoBehaviour

        private void OnDestroy()
        {
            _disposable.Clear();
            _cts?.Cancel();
        }

        #endregion

        public void Initialize(Hero hero)
        {
            _hero = hero;
            _thisTransform = transform;
            _heroTransform = _hero.transform;

            _canvas.worldCamera = Camera.main;
            
            Subscribe();
        }

        private void Subscribe()
        {
            _hero.DashedCommand
                .Where(_ => _displaying == false)
                .Subscribe(_ => Display().Forget())
                .AddTo(_disposable);

            this.UpdateAsObservable()
                .Where(_ => _displaying == true)
                .Subscribe(_ => FollowHero())
                .AddTo(_disposable);
        }

        private async UniTask Display()
        {
            _cts = new();
            _animatedUI.Reveal(_cts.Token).Forget();
            _displaying = true;

            await MyUniTask
                .Delay(_config.DashDuration, _cts.Token)
                .SuppressCancellationThrow();

            await AnimateCooldown(_cts.Token)
                .SuppressCancellationThrow();

            await _animatedUI.Conceal(_cts.Token);
            _displaying = false;
        }

        private void FollowHero()
        {
            Vector2 position = (Vector2)_heroTransform.position + _positionOffset;
            _thisTransform.position = position;
        }

        private async UniTask AnimateCooldown(CancellationToken token)
        {
            float duration = _config.DashCooldown;

            bool canceled = false;
            float elapsedTime = 0f;
            while (canceled == false && elapsedTime < duration)
            {
                float delta = elapsedTime / duration;
                _slider.value = delta;

                elapsedTime += Time.deltaTime;

                canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }
        }
    }
}