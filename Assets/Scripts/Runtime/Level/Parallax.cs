using System;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Core.Level
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private Transform _camera;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField, Range(0f, 1f)] private float _effect;

        private readonly CompositeDisposable _disposable = new();
        private float _length;
        private Transform _thisTransform;
        private float _startX;
        private float _temp;

        #region MonoBehaviour

        private void Awake()
        {
            _thisTransform = transform;
            _startX = _thisTransform.position.x;
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
                .Where(_ => _temp > _startX + _length)
                .Subscribe(_ => MoveRight())
                .AddTo(_disposable);

            fixedUpdate
                .Where(_ => _temp < _startX - _length)
                .Subscribe(_ => MoveLeft())
                .AddTo(_disposable);
        }

        private void Perform()
        {
            float distance = _camera.position.x * _effect;
            _temp = _camera.position.x * (1 - _effect);

            Vector2 parallaxed = GetParallaxedPosition(distance);
            _thisTransform.position = parallaxed;
        }

        private void MoveRight() =>
            _startX += _length;

        private void MoveLeft() =>
            _startX += _length;

        private Vector2 GetParallaxedPosition(float distance)
        {
            Vector2 parallaxed = _thisTransform.position;
            parallaxed.x = _startX + distance;
            return parallaxed;
        }
    }
}