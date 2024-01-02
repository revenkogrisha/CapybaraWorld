using Core.Other;
using DG.Tweening;
using NTC.Pool;
using UnityEngine;

namespace Core.Level
{
    public class Food : Entity, ICollectable, ISpawnable
    {
        [SerializeField] private float _fadeDuration = 0.3f;

        public bool CanCollect { get; private set; } = true;

        private void Awake()
        {
            if (Preloaded == true)
                Setup();
        }

        public void OnSpawn() => 
            Setup();

        public void OnCollected()
        {
            CanCollect = false;
            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, _fadeDuration))
                .AppendCallback(Despawn);
        }

        private void Setup()
        {
            transform.localScale = Vector2.one;
            CanCollect = true;
        }

        private void Despawn() =>
            gameObject.SelfDespawn();
    }
}
