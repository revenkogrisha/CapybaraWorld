using System;
using System.Threading;
using Core.Editor.Debugger;
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

            try
            {
                while (true)
                {
                    float targetX = _targetTransfrom.position.x;
                    int playthroughDistance = Mathf.RoundToInt(targetX * ValueMultiplier);
                    if (playthroughDistance < 0)
                        playthroughDistance = 0;

                    _score.PlaythroughScore.Value = playthroughDistance;

                    if (token.IsCancellationRequested == true)
                    {
                        _cts?.Dispose();
                        _cts = null;
                    }

                    await UniTaskUtility.Delay(UpdateFrequency, token);
                }
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(DistanceScoreCounter)}::{nameof(Count)}: {ex.Message} \n{ex.StackTrace}");
            }
            finally
            {
                _cts.Clear();
                _cts = null;
            }
        }
    }
}
