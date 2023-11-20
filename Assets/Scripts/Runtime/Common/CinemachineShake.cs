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
        private CancellationTokenSource _cts;

        public CinemachineShake(CinemachineVirtualCamera cinemachine)
        {
            _perlinNoise = cinemachine
                .GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            
            Reset();
        }

        public void Dispose() => 
            _cts.Clear();

        public async UniTaskVoid Shake(float intensity, float duration)
        {
            _cts = new();
            CancellationToken token = _cts.Token;

            try
            {
                _perlinNoise.m_AmplitudeGain = intensity;

                await MyUniTask.Delay(duration, token);
                Reset();
            }
            catch
            {
                _cts.Clear();
            }
        }

        private void Reset() =>
            _perlinNoise.m_AmplitudeGain = 0f;
    }
}
