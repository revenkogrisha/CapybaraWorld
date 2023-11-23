using Core.Level;
using Core.Other;
using NTC.Pool;
using UnityEngine;

namespace Core.Factories
{
    public class CoinFactory : IFactory<Coin>
    {
        private readonly Coin _coinPrefab;

        public CoinFactory(Coin coinPrefab)
        {
            _coinPrefab = coinPrefab;
        }

        public Coin Create(Transform root, Vector2 position)
        {
            Coin coin = Create();

            coin.SetParent(root);
            coin.SetLocalPosition(position);

            return coin;
        }

        public Coin Create()
        {
            Coin coin = NightPool.Spawn(_coinPrefab);
            return coin;
        }
    }
}
