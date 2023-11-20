using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Core.Other;

namespace Core.Level
{
    public class BackgroundHandler : IDisposable
    {
        private const float BackgroundLerpDuration = 3.5f;

        private readonly Color _defaultBackground;
        private readonly Camera _camera;
        private CancellationTokenSource _cts;

        public BackgroundHandler()
        {
            _camera = Camera.main;
            _defaultBackground = _camera.backgroundColor;
        }

        public void Dispose() =>
            _cts.Clear();

        public void ChangeBackgroundColor(Location location)
        {
            Color newBackground = location.UseDefaultBackground == true  
                ? _defaultBackground
                : location.BackgroundColor;

            if (_camera.backgroundColor == newBackground)
                return;

            LerpBackground(newBackground).Forget();
        }

        private async UniTaskVoid LerpBackground(Color newBackground)
        {
            _cts = new();
            CancellationToken token = _cts.Token;

            float elapsedTime = 0;
            Color currentBackground = _camera.backgroundColor;

            while (elapsedTime < BackgroundLerpDuration)
            {
                float time = elapsedTime / BackgroundLerpDuration;
                _camera.backgroundColor = Color.Lerp(currentBackground, newBackground, time);

                elapsedTime += Time.deltaTime;

                await UniTask.NextFrame(token);
            }
        }
    }
}
