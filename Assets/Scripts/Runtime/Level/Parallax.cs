using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core.Level
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private bool _applyX = true;
        [SerializeField, Range(0f, 1f)] private float _effectX;
        private bool _applyY = false;
        [SerializeField, Range(0f, 1f)] private float _effectY;

        private readonly CompositeDisposable _disposable = new();
        private float _length;
        private Transform _thisTransform;
        private Vector2 _startPosition;
        private float _temp;

        #region MonoBehaviour

        private void Awake()
        {
            _thisTransform = transform;
            _startPosition = _thisTransform.position;
            _length = _spriteRenderer.bounds.size.x;
        }

        private void OnEnable() => 
            SubscribeFixedUpdate();

        private void OnDisable() => 
            _disposable.Clear();

        #endregion

        private void SubscribeFixedUpdate()
        {
            IObservable<Unit> fixedUpdate = this.FixedUpdateAsObservable();
            fixedUpdate
                .Subscribe(_ => Perform())
                .AddTo(_disposable);

            fixedUpdate
                .Where(_ => _temp > _startPosition.x + _length)
                .Subscribe(_ => MoveRight())
                .AddTo(_disposable);

            fixedUpdate
                .Where(_ => _temp < _startPosition.x - _length)
                .Subscribe(_ => MoveLeft())
                .AddTo(_disposable);
        }

        private void Perform()
        {
            _temp = _camera.position.x * (1 - _effectX);

            Vector2 parallaxed = GetParallaxedPosition();
            _thisTransform.position = parallaxed;
        }

        private void MoveRight() =>
            _startPosition.x += _length;

        private void MoveLeft() =>
            _startPosition.x -= _length;

        private Vector2 GetParallaxedPosition()
        {
            Vector2 parallaxed = _thisTransform.position;
            if (_applyX == true)
            {
                float distanceX = _camera.position.x * _effectX;
                parallaxed.x = _startPosition.x + distanceX;
            }

            if (_applyY == true)
            {
                float distanceY = _camera.position.y * _effectY;
                parallaxed.y = _startPosition.y + distanceY;
            }

            return parallaxed;
        }
    }
}