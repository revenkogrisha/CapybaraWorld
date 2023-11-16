using System.Threading;
using Core.Other;
using Core.UI;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Player
{
    public class DashRecoveryDisplay : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        [SerializeField] private Slider _slider;
        [SerializeField] private AnimatedUI _animatedUI;
        [SerializeField] private HeroConfig _config;

        private readonly CompositeDisposable _disposable = new();
        private CancellationToken _cancellationToken;
        private bool _displaying = false;

        #region MonoBehaviour

        private void Awake() =>
            _cancellationToken = destroyCancellationToken;

        private void OnEnable()
        {
            _hero.DashedCommand
                .Where(_ => _displaying == false)
                .Subscribe(_ => Display(_cancellationToken).Forget())
                .AddTo(_disposable);
        }

        private void OnDisable() =>
            _disposable.Clear();

        #endregion

        private async UniTaskVoid Display(CancellationToken token)
        {
            _animatedUI.Reveal().Forget();
            _displaying = true;

            bool dashCanceled = await MyUniTask
                .Delay(_config.DashDuration, token)
                .SuppressCancellationThrow();

            if (dashCanceled == true)
                return;

            await AnimateCooldown(token)
                .SuppressCancellationThrow();

            _animatedUI.Conceal().Forget();
            _displaying = false;
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