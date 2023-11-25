using System;
using UniRx;

namespace Core.Player
{
    public class PlayerData : IDisposable
    {
        private const int CoinsDefaultReward = 1;
        private const int FoodDefaultReward = 1;
        
        private readonly CompositeDisposable _disposable = new();

        public int CoinsAmount { get; private set; }
        public int FoodAmount { get; private set; }

        public void Dispose() => 
            _disposable.Clear();

        public void Initialize(IPlayerEventsHandler playerEvents)
        {
            playerEvents.CoinCollectedCommand
                .Subscribe(_ => AddCoin())
                .AddTo(_disposable);

            playerEvents.FoodCollectedCommand
                .Subscribe(_ => AddFood())
                .AddTo(_disposable);
        }

        private void AddCoin() =>
            CoinsAmount += CoinsDefaultReward;

        private void AddFood() =>
            FoodAmount += FoodDefaultReward;
    }
}