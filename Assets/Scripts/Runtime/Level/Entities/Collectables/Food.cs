using Core.Other;
using DG.Tweening;
using NTC.Pool;
using UnityEngine;

namespace Core.Level
{
    public class Food : Entity, ISpawnable
    {
        [SerializeField] private float _fadeDuration = 0.3f;

        private bool _canEat = true;

        public void OnSpawn()
        {
            transform.localScale = Vector2.one;
            _canEat = true;
        }

        public void GetEatten()
        {
            if (_canEat == false)
                return;

            _canEat = false;
            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, _fadeDuration))
                .AppendCallback(Despawn);
        }

        private void Despawn() =>
            gameObject.SelfDespawn();
    }
}
