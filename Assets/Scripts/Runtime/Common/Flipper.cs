using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Core.Other;

namespace Core.Common
{
    public class Flipper : MonoBehaviour
    {
        private const float CheckFrequency = 0.15f;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Space]
        [SerializeField] private float _minimumVelocityToFlip = 5f;

        private Transform _thisTransform;
        private LookingDirection _direction = LookingDirection.Right;

        private bool FacingRight => _direction == LookingDirection.Right;
        private bool FacingLeft => _direction == LookingDirection.Left;
        private bool ShouldFlip => ShouldFlipLeft == true || ShouldFlipRight == true;
        private bool ShouldFlipRight => _rigidbody2D.velocity.x > _minimumVelocityToFlip && FacingLeft == true;
        private bool ShouldFlipLeft => _rigidbody2D.velocity.x < -_minimumVelocityToFlip && FacingRight == true;
        
        #region MonoBehaviour

        private void Awake() =>
            _thisTransform = transform;

        private void Start() => 
            CheckRigidbodyVelocity().Forget();

        #endregion

        private async UniTaskVoid CheckRigidbodyVelocity()
        {
            CancellationToken token = destroyCancellationToken;
            while (true)
            {
                if (ShouldFlip == true)
                    Flip();

                bool canceled = await MyUniTask
                    .Delay(CheckFrequency, token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }

        private void Flip()
        {
            Vector2 flipped = _thisTransform.localScale;
            flipped.x *= -1f;

            _thisTransform.localScale = flipped;

            _direction = FacingLeft == true
                ? _direction = LookingDirection.Right
                : _direction = LookingDirection.Left;
        }
    }
}
