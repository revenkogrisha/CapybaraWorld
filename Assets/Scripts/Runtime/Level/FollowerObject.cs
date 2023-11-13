using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Other;
using System;
using System.Threading;
using Object = UnityEngine.Object;

namespace Core.Level
{
    public class FollowerObject : IDisposable
    {
        private float _updateIntervalInSeconds = 1f;
        private Transform _transformToFollow;
        private Transform _thisTransform;
        private CancellationTokenSource _cancellationTokenSource;

        public bool IgnoreXMovement { get; set; }
        public bool IgnoreYMovement { get; set; }

        public Transform ObjectToFollow
        {
            get => _transformToFollow;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(
                        $"{value.GetType().FullName} in {GetType().FullName}");

                _transformToFollow = value;
            }
        }

        public FollowerObject(
            Transform thisTransform,
            bool ignoreXMovement = false,
            bool ignoreYMovement = true,
            float updateIntervalInSeconds = 0.5f)
        {
            IgnoreXMovement = ignoreXMovement;
            IgnoreYMovement = ignoreYMovement;
            _updateIntervalInSeconds = updateIntervalInSeconds;

            _thisTransform = thisTransform;
        }

        public void Dispose()
        {
            Object.Destroy(_thisTransform.gameObject);
            
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void BeginFollowing(Transform toFollow)
        {
            _transformToFollow = toFollow;
            Follow().Forget();
        }

        private async UniTask Follow()
        {
            _cancellationTokenSource = new();
            CancellationToken token = _cancellationTokenSource.Token;

            bool canceled = false;
            while (canceled == false)
            {
                Vector3 movedPosition = GetMovedPosition();
                _thisTransform.position = movedPosition;

                canceled = await MyUniTask
                    .Delay(_updateIntervalInSeconds, token)
                    .SuppressCancellationThrow();
            }

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private Vector3 GetMovedPosition()
        {
            Vector3 movedPosition = _thisTransform.position;
            if (IgnoreXMovement == false)
                movedPosition = MoveX(movedPosition);

            if (IgnoreYMovement == false)
                movedPosition = MoveY(movedPosition);

            return movedPosition;
        }

        private Vector2 MoveX(Vector2 movedPosition)
        {
            Vector3 toFollowPosition = ObjectToFollow.position;
            movedPosition.x = toFollowPosition.x;

            return movedPosition;
        }

        private Vector2 MoveY(Vector2 movedPosition)
        {
            Vector3 toFollowPosition = ObjectToFollow.position;
            movedPosition.y = toFollowPosition.y;

            return movedPosition;
        }
    }
}