using System.Threading;
using Core.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Game
{
    public class YPositionScoreCounter : IScoreCounter
    {
        private const float UpdateFrequency = 1f;

        private readonly Transform _targetTransfrom;
        private readonly Score _score;
        private CancellationTokenSource _cancellationSource;

        public YPositionScoreCounter(Transform target, Score score) =>
            _targetTransfrom = target;

        public void Dispose() => 
            StopCount();

        public void StartCount() =>
            Count().Forget();

        public void StopCount() => 
            _cancellationSource?.Cancel();

        private async UniTaskVoid Count()
        {
            _cancellationSource = new();
            CancellationToken token = _cancellationSource.Token;

            bool canceled = false;
            while (canceled == false)
            {
                float targetY = _targetTransfrom.position.y;
                int playthroughDistance = Mathf.RoundToInt(targetY);

                _score.PlaythroughScore = playthroughDistance;

                bool isHighestScore = _score.PlaythroughScore > _score.HighestScore;
                if (isHighestScore == true)
                    _score.CaptureHighestScore();

                canceled = await MyUniTask
                    .Delay(UpdateFrequency, token)
                    .SuppressCancellationThrow();
            }

            _cancellationSource?.Dispose();
            _cancellationSource = null;
        }
    }
}
