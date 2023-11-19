using System;
using System.Threading;
using Cinemachine;
using Core.Other;
using Cysharp.Threading.Tasks;

namespace Core.Common
{
    public class CinemachineShake : IDisposable
    {
        private readonly CinemachineBasicMultiChannelPerlin _perlinNoise;
        private CancellationTokenSource _cancellationSource;

        public CinemachineShake(CinemachineVirtualCamera cinemachine)
        {
            _perlinNoise = cinemachine
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            Reset();
        }

        public void Dispose()
        {
            _cancellationSource?.Cancel();
            _cancellationSource?.Dispose();
            _cancellationSource = null;
        }

        public async UniTaskVoid Shake(float intensity, float duration)
        {
            _cancellationSource = new();
            CancellationToken token = _cancellationSource.Token;

            _perlinNoise.m_AmplitudeGain = intensity;

            try
            {
                await MyUniTask.Delay(duration, token);
                Reset();
            }
            finally
            {
                _cancellationSource?.Dispose();
                _cancellationSource = null;
            }
        }

        private void Reset() =>
            _perlinNoise.m_AmplitudeGain = 0f;
    }
}
