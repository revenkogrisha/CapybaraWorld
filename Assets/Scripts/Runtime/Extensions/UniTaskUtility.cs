using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Other
{
    public static class UniTaskUtility
    {
        public static UniTask Delay(float seconds, CancellationToken token)
        {
            TimeSpan delay = TimeSpan.FromSeconds(seconds);
            return UniTask.Delay(delay, cancellationToken: token);
        }
    }
}
