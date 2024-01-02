using System;
using Core.Factories;
using Core.Other;
using DG.Tweening;
using NTC.Pool;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Level
{
    public class Chest : Entity, ISpawnable
    {
        private const float DespawnDelay = 2f;
        
        [SerializeField] private ChestPreset _preset;
        [SerializeField] private ParticleSystem[] _particles;
        
        private CoinFactory _coinFactory;
        private bool _canOpen = true;

        private void Awake()
        {
            if (Preloaded == true)
                Setup();
        }

        public void OnSpawn()
        {
            Setup();
        }

        public void Open()
        {
            if (_canOpen == false)
                return;

            for (int i = 0; i < _preset.CoinsInside; i++)
                PushOutCoin();

            _canOpen = false;

            foreach (ParticleSystem system in _particles)
                system.Stop();
                
            transform.DOScale(Vector2.zero, _preset.FadeDuration);

            Invoke(nameof(Despawn), DespawnDelay);
        }

        private void Setup()
        {
            _coinFactory = new(_preset.CoinPrefab);
            transform.localScale = Vector2.one;
            _canOpen = true;
        }
        
        private void PushOutCoin()
        {
            Coin coin = _coinFactory.Create(transform.parent, transform.localPosition);
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