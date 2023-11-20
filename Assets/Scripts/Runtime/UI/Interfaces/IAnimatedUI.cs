using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.UI
{
    public interface IAnimatedUI
    {
        public UniTask Reveal(CancellationToken token);

        public UniTask Conceal(CancellationToken token);
    }
}
