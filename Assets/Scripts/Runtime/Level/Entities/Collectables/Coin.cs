using Core.Other;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using NTC.Pool;
using Unity.Mathematics;
using UnityEngine;

namespace Core.Level
{
    public class Coin : MonoBehaviour, IDespawnable
    {
        private const float BlockDuration = 1.5f;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private LayerMask _heroLayer;

        public Rigidbody2D Rigidbody2D => _rigidbody2D;

        public void OnDespawn()
        {
            transform.rotation = quaternion.identity;
            _rigidbody2D.velocity = Vector2.zero;
        }
        
        public void Initialize() => 
            BlockCollecting().Forget();

        public void GetCollected()
        {
            DOTween.Sequence()
                .Append(transform.DOScale(Vector2.zero, 0.2f))
                .AppendCallback(() => NightPool.Despawn(gameObject));
        }

        private async UniTaskVoid BlockCollecting()
        {
            _rigidbody2D.excludeLayers = _heroLayer;
            _rigidbody2D.includeLayers = 0;

            await MyUniTask.Delay(BlockDuration, destroyCancellationToken);

            _rigidbody2D.excludeLayers = 0;
            _rigidbody2D.includeLayers = _heroLayer;
        }
    }
}
