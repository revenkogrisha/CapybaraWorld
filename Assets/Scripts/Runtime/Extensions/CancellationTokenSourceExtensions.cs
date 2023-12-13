using System.Threading;

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
    }
}
