using Core.Other;
using DG.Tweening;
using UnityEngine;

namespace Core.Level
{
    public class Food : MonoBehaviour 
    {
        [SerializeField] private float _fadeDuration = 0.3f;

        public void GetEatten()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, _fadeDuration))
                .AppendCallback(Despawn);
        }

        private void Despawn() =>
            gameObject.SelfDespawn();
    }
}
