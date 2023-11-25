using UniRx;

namespace Core.Player
{
    public interface IPlayerEventsHandler
    {
        public ReactiveCommand CoinCollectedCommand { get; }
        public ReactiveCommand FoodCollectedCommand { get; }
    }
}