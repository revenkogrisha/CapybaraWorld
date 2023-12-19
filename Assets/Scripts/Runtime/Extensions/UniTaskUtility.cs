using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Other
{
    public static class UniTaskUtility
    {
        public static UniTask Delay(int seconds) => 
            Delay((float)seconds);

        public static UniTask Delay(int seconds, CancellationToken token) => 
            Delay((float)seconds, token);

        public static UniTask Delay(float seconds)
        {
            TimeSpan delay = TimeSpan.FromSeconds(seconds);
            return UniTask.Delay(delay);
        }

        public static UniTask Delay(float seconds, CancellationToken token)
        {
            TimeSpan delay = TimeSpan.FromSeconds(seconds);
            return UniTask.Delay(delay, cancellationToken: token);
        }
    }
}
