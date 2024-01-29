using System.Threading;
using Core.Editor.Debugger;
using Cysharp.Threading.Tasks;

namespace Core.Common.ThirdParty
{
    public class FakeUserReviewService : IUserReviewService
    {
        public bool IsFake { get; } = true;
        public void Initialize() =>
            RDebug.Info($"{nameof(FakeUserReviewService)}::{nameof(Initialize)}");

        public async UniTaskVoid Request(CancellationToken token) 
        {
            await UniTask.NextFrame();
            RDebug.Info($"{nameof(FakeUserReviewService)}::{nameof(Request)}");
        }
    }
}