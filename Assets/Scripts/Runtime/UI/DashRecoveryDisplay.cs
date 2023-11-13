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
        [SerializeField] private PlayerConfig _config;

        private CompositeDisposable _disposable = new();
        private bool _displaying = false;

        #region MonoBehaviour

        private void OnEnable()
        {
            _hero.DashedCommand
                .Where(_ => _displaying == false)
                .Subscribe(_ => Display())
                .AddTo(_disposable);
        }

        private void OnDisable() =>
            _disposable.Clear();

        #endregion

        private async void Display()
        {
            CancellationToken token = destroyCancellationToken;
            _animatedUI.Reveal().Forget();
            _displaying = true;

            bool dashCanceled = await MyUniTask
                .Delay(_config.DashDuration, token)
                .SuppressCancellationThrow();

            if (dashCanceled == true)
                return;

            bool canceled = false;
            float duration = _config.DashCooldown;
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

            _animatedUI.Conceal().Forget();
            _displaying = false;
        }
    }
}