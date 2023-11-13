using Cysharp.Threading.Tasks;

namespace Core.UI
{
    public interface IAnimatedUI
    {
        public UniTask Reveal();

        public UniTask Conceal();
    }
}
