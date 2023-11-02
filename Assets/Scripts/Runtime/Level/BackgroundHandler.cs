using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Level
{
    public class BackgroundHandler : IDisposable
    {
        private const float BackgroundLerpDuration = 3.5f;

        private readonly Color _defaultBackground;
        private readonly Camera _camera;
        private CancellationTokenSource _cancellationTokenSource;

        public BackgroundHandler()
        {
            _camera = Camera.main;
            _defaultBackground = _camera.backgroundColor;
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void ChangeBackgroundColor(Location location)
        {
            Color newBackground = location.UseDefaultBackground == true  
                ? _defaultBackground
                : location.BackgroundColor;

            if (_camera.backgroundColor == newBackground)
                return;

            LerpBackgroundAsync(newBackground).Forget();
        }

        private async UniTask LerpBackgroundAsync(Color newBackground)
        {
            _cancellationTokenSource = new();
            CancellationToken token = _cancellationTokenSource.Token;

            float elapsedTime = 0;
            Color currentBackground = _camera.backgroundColor;
            while (elapsedTime < BackgroundLerpDuration)
            {
                if (token.IsCancellationRequested == true)
                    break;

                float time = elapsedTime / BackgroundLerpDuration;
                _camera.backgroundColor = Color.Lerp(currentBackground, newBackground, time);

                elapsedTime += Time.deltaTime;

                await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();
            }

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }
    }
}
