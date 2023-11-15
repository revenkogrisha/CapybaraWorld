using System.Threading;
using Core.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Game
{
    public class XPositionScoreCounter : IScoreCounter
    {
        private const float UpdateFrequency = 1f;
        private const float ValueMultiplier = 0.2f;

        private readonly Transform _targetTransfrom;
        private readonly Score _score;
        private CancellationTokenSource _cancellationSource;

        public XPositionScoreCounter(Transform target, Score score)
        {
            _targetTransfrom = target;
            _score = score;
        }

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
                float targetX = _targetTransfrom.position.x;
                int playthroughDistance = Mathf.RoundToInt(targetX * ValueMultiplier);
                if (playthroughDistance < 0)
                    playthroughDistance = 0;

                _score.PlaythroughScore.Value = playthroughDistance;

                bool isHighestScore = _score.PlaythroughScore.Value > _score.HighestScore;
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
