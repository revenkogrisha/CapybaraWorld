using System.Threading;
using Cysharp.Threading.Tasks;
using Google.Play.AppUpdate;

namespace Core.Common
{
    public interface IAppUpdateService
    {
        public void Initialize();
        public UniTask<UpdateAvailability> StartUpdateCheck(CancellationToken token = default);
        public UniTask Request(CancellationToken token = default);
    }
}