using Core.Other;
using DG.Tweening;
using NTC.Pool;
using UnityEngine;

namespace Core.Level
{
    public class Food : Entity, ISpawnable
    {
        [SerializeField] private float _fadeDuration = 0.3f;

        public bool CanCollect { get; private set; } = true;

        public void OnSpawn()
        {
            transform.localScale = Vector2.one;
            CanCollect = true;
        }

        public void GetCollected()
        {
            CanCollect = false;
            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, _fadeDuration))
                .AppendCallback(Despawn);
        }

        private void Despawn() =>
            gameObject.SelfDespawn();
    }
}
