using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.UI
{
    public interface IAnimatedUI
    {
        public UniTask Reveal(CancellationToken token, bool enable);
        public UniTask Conceal(CancellationToken token, bool disable);
    }
}
