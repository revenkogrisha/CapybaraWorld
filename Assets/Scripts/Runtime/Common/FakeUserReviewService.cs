using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Common
{
    public class FakeUserReviewService : IUserReviewService
    {
        public bool IsFake { get; } = true;
        public void Initialize() {  }

        public async UniTaskVoid Request(CancellationToken token) {  }
    }
}