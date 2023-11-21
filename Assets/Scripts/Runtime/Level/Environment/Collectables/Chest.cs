using Core.Factories;
using Core.Other;
using DG.Tweening;
using UnityEngine;

namespace Core.Level
{
    public class Chest : MonoBehaviour
    {
        private const float DespawnDelay = 3f;
        
        [SerializeField] private ChestPreset _preset;
        [SerializeField] private ParticleSystem _particles;
        
        private CoinFactory _coinFactory;
        private bool _canOpen = true;

        private void Awake() => 
            _coinFactory = new(_preset.CoinPrefab, transform.parent, transform.position);

        public void Open()
        {
            if (_canOpen == false)
                return;

            for (int i = 0; i < _preset.CoinsInside; i++)
                PushOutCoin();

            _canOpen = false;

            _particles.Stop();
            transform.DOScale(Vector2.zero, _preset.FadeDuration);

            Invoke(nameof(Despawn), DespawnDelay);
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
            VectorRange pushRange = _preset.PushRange;
            float randomX = Random.Range(pushRange.Minimum.x, pushRange.Maximum.x);
            float randomY = Random.Range(pushRange.Minimum.y, pushRange.Maximum.y);
            return new(randomX, randomY);
        }

        private float GetPushForce() => 
            Random.Range(_preset.MinimumPushForce, _preset.MaximumPushForce);

        private void Despawn() =>
            gameObject.SelfDespawn();
    }
}