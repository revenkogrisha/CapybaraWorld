using System.Threading;
using Core.Common.ThirdParty;
using Core.Other;
using Cysharp.Threading.Tasks;
using YG;

namespace Core
{
    public class YGReviewService : IUserReviewService
    {
        public bool IsFake => throw new System.NotImplementedException();

        public void Initialize() {  }

        public async UniTaskVoid Request(CancellationToken token)
        {
            await UniTaskUtility.Delay(0.5f, token);
            YandexGame.ReviewShow(true);
        }
    }
}