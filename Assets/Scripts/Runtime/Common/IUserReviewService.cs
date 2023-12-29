using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Common
{
    public interface IUserReviewService
    {
        public void Initialize();
        public UniTaskVoid Request(CancellationToken token);
    }
}