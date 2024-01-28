using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Common
{
    public class FakeUserReviewService : IUserReviewService
    {
        public bool IsFake { get; } = true;
        public void Initialize() {  }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async UniTaskVoid Request(CancellationToken token) {  }
#pragma warning restore CS1998
    }
}