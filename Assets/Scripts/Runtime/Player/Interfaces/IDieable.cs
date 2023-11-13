using UniRx;

namespace Core.Player
{
    public interface IDieable
    {
        public ReactiveProperty<bool> IsDead { get; }
    }
}
