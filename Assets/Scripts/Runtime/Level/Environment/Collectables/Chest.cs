using Core.Factories;
using DG.Tweening;
using UnityEngine;

namespace Core.Level
{
    public class Chest : MonoBehaviour
    {
        private CoinFactory _coinFactory;
        [SerializeField] private ChestPreset _preset;

        private void Awake() => 
            _coinFactory = new(_preset.CoinPrefab, transform.parent, transform.position);

        public void Open()
        {
            for (int i = 0; i < _preset.CoinsInside; i++)
                PushOutCoin();

            transform.DOScale(Vector2.zero, 0.3f);
        }

        private void PushOutCoin()
        {
            Coin coin = _coinFactory.Create();
            coin.Initialize();

            Vector2 pushVector = GetPushVector();
            float pushForce = GetPushForce();

            coin.Rigidbody2D.velocity = pushVector * pushForce;
        }

        private Vector2 GetPushVector()
        {
            float randomX = Random.Range(-1f, 1f);
            float randomY = Random.Range(0f, 1f);
            return new(randomX, randomY);
        }

        private float GetPushForce() => 
            Random.Range(_preset.MinimumPushForce, _preset.MaximumPushForce);
    }
}