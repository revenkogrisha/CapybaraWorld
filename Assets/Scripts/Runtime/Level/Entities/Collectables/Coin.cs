using Core.Other;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NTC.Pool;
using UnityEngine;

namespace Core.Level
{
    public class Coin : MonoBehaviour, IDespawnable
    {
        private const float BlockDuration = 1f;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private LayerMask _heroLayer;

        [Space]
        [SerializeField] private float _fadeDuration = 0.2f;

        public bool CanCollect { get; private set; } = true;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        public void OnDespawn()
        {
            transform.rotation = Quaternion.identity;
            _rigidbody2D.velocity = Vector2.zero;
            CanCollect = true;
        }
        
        public void Initialize() => 
            BlockCollecting().Forget();

        public void GetCollected()
        {
            CanCollect = false;
            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, _fadeDuration))
                .AppendCallback(() => gameObject.SelfDespawn());
        }

        private async UniTaskVoid BlockCollecting()
        {
            CanCollect = false;
            _rigidbody2D.excludeLayers = _heroLayer;
            _rigidbody2D.includeLayers = 0;

            await MyUniTask.Delay(BlockDuration, destroyCancellationToken);

            _rigidbody2D.excludeLayers = 0;
            _rigidbody2D.includeLayers = _heroLayer;
            CanCollect = true;
        }
    }
}
