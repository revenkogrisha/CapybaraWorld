using System.Threading;
using Core.Other;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Game
{
    public class DistanceScoreCounter : IScoreCounter
    {
        private const float UpdateFrequency = 1f;
        private const float ValueMultiplier = 0.2f;

        private readonly Transform _targetTransfrom;
        private readonly Score _score;
        private CancellationTokenSource _cts;

        public DistanceScoreCounter(Transform target, Score score)
        {
            _targetTransfrom = target;
            _score = score;
        }

        public void Dispose() => 
            StopCount();

        public void StartCount() =>
            Count().Forget();

        public void StopCount() => 
            _cts.Clear();

        private async UniTaskVoid Count()
        {
            _cts = new();
            CancellationToken token = _cts.Token;

            while (true)
            {
                float targetX = _targetTransfrom.position.x;
                int playthroughDistance = Mathf.RoundToInt(targetX * ValueMultiplier);
                if (playthroughDistance < 0)
                    playthroughDistance = 0;

                _score.PlaythroughScore.Value = playthroughDistance;

                bool isHighestScore = _score.PlaythroughScore.Value > _score.HighestScore;
                if (isHighestScore == true)
                    _score.CaptureHighestScore();

                await MyUniTask.Delay(UpdateFrequency, token);
            }
        }
    }
}
