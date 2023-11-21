using Core.Level;
using NTC.Pool;
using UnityEngine;

namespace Core.Factories
{
    public class CoinFactory : IFactory<Coin>
    {
        private readonly Coin _coinPrefab;
        private readonly Transform _root;
        private readonly Vector2 _position;

        public CoinFactory(Coin coinPrefab, Transform root, Vector2 position)
        {
            _coinPrefab = coinPrefab;
            _root = root;
            _position = position;
        }

        public Coin Create()
        {
            Coin coin = NightPool.Spawn(_coinPrefab, _root);
            coin.transform.localPosition = _position;
            return coin;
        }
    }
}
