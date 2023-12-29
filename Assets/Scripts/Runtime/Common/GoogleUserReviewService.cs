#if !UNITY_EDITOR && UNITY_ANDROID
using System;
using System.Collections;
using System.Threading;
using Core.Editor.Debugger;
using Cysharp.Threading.Tasks;
using Google.Play.Common;
using Google.Play.Review;

namespace Core.Common
{
    public class GoogleUserReviewService : IUserReviewService
    {
        private ReviewManager _manager;
        
        public void Initialize() => 
            _manager = new();

        public async UniTaskVoid Request(CancellationToken token)
        {
            try
            {
                await RequestCoroutine().ToUniTask(cancellationToken: token);
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(GoogleUserReviewService)}::{nameof(Request)}: {ex.Message} \n{ex.StackTrace}");
            }
        }

        private IEnumerator RequestCoroutine()
        {
            string log = $"{nameof(GoogleUserReviewService)}::{nameof(RequestCoroutine)}";
            
            PlayAsyncOperation<PlayReviewInfo, ReviewErrorCode> requestFlowOperation = _manager.RequestReviewFlow();
            yield return requestFlowOperation;
            
            if (requestFlowOperation.Error != ReviewErrorCode.NoError)
            {
                RDebug.Error($"{log}: Request Flow error was caught: {requestFlowOperation.Error.ToString()}");
                yield break;
            }
            
            PlayReviewInfo info = requestFlowOperation.GetResult();
            
            PlayAsyncOperation<VoidResult, ReviewErrorCode> launchFlowOperation = _manager.LaunchReviewFlow(info);
            yield return launchFlowOperation;
            
            info = null;
            if (launchFlowOperation.Error != ReviewErrorCode.NoError)
            {
                RDebug.Error($"{log}: Review Error was caught: {launchFlowOperation.Error.ToString()}");
                yield break;
            }
        }
    }
}
#endif