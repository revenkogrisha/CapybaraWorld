using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Other
{
    public static class CancellationTokenSourceExtensions
    {
        public static void Clear(this CancellationTokenSource cts)
        {
            if (cts == null)
                return;
            
            if (cts.IsCancellationRequested == false)
                cts.Cancel();
            
            cts.Dispose();
        }
        
        
        public static async UniTaskVoid CancelByTimeout(this CancellationTokenSource cts, float timeout)
        {
            float timeoutTime = Time.time + timeout;
            while (true)
            {
                if (cts.IsCancellationRequested == false && Time.time >= timeoutTime)
                {
                    cts.Clear();
                    return;
                }

                await UniTask.NextFrame();
            }
        }
    }
}
