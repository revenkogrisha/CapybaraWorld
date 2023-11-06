using Cysharp.Threading.Tasks;

namespace Core.UI
{
    public interface IAnimatedView
    {
        public UniTask RevealAsync();

        public UniTask ConcealAsync();
    }
}
