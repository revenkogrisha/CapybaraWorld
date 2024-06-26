using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Common.ThirdParty
{
    public interface IUserReviewService
    {
        public bool IsFake { get; } 
        
        public void Initialize();
        public UniTaskVoid Request(CancellationToken token);
    }
}