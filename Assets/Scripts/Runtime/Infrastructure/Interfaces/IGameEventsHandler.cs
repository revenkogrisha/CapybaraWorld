using UniRx;

namespace Core.Game
{
    public interface IGameEventsHandler
    {
        public ReactiveCommand GameWinCommand { get; }
    }
}
