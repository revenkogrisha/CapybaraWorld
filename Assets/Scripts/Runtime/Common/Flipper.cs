using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using Core.Other;
using System;

namespace Core.Common
{
    public class Flipper : MonoBehaviour
    {
        private const float CheckFrequency = 0.15f;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Space]
        [SerializeField] private float _minimumVelocityToFlip = 5f;
        [SerializeField] private LookingDirection _startDirection = LookingDirection.Right;

        [Space]
        [SerializeField] private bool _lerpFlipping = true;
        [SerializeField] private float _lerpDuration = 0.4f;

        private Transform _thisTransform;
        private LookingDirection _direction;

        private bool FacingRight => _direction == LookingDirection.Right;
        private bool FacingLeft => _direction == LookingDirection.Left;
        private bool ShouldFlip => ShouldFlipLeft == true || ShouldFlipRight == true;
        private bool ShouldFlipRight => _rigidbody2D.velocity.x > _minimumVelocityToFlip && FacingLeft == true;
        private bool ShouldFlipLeft => _rigidbody2D.velocity.x < -_minimumVelocityToFlip && FacingRight == true;

        public event Action StartFlipping;

        #region MonoBehaviour

        private void Awake()
        {
            _thisTransform = transform;
            _direction = _startDirection;
        }

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

            if (_lerpFlipping == true)
                LerpFlip(flipped).Forget();
            else
                _thisTransform.localScale = flipped;

            _direction = FacingLeft == true
                ? _direction = LookingDirection.Right
                : _direction = LookingDirection.Left;
        }

        private async UniTaskVoid LerpFlip(Vector2 flipped)
        {
            CancellationToken token = destroyCancellationToken;
            StartFlipping?.Invoke();

            float elapsedTime = 0f;
            while (elapsedTime < _lerpDuration)
            {
                Vector2 newScale = _thisTransform.localScale;
                newScale.x = flipped.x;

                float delta = elapsedTime / _lerpDuration;
                _thisTransform.localScale = Vector2.Lerp(
                    _thisTransform.localScale,
                    newScale,
                    delta);

                elapsedTime += Time.deltaTime;

                bool canceled = await UniTask
                    .NextFrame(token)
                    .SuppressCancellationThrow();

                if (canceled == true)
                    break;
            }
        }
    }
}