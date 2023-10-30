using UnityEngine;
using Cysharp.Threading.Tasks;
using Core.Other;
using System;

namespace Core.Level
{
    public class FollowerObject : MonoBehaviour
    {
        private readonly float _updateIntervalInSeconds = 1f;
        private Transform _objectToFollow;
        private Transform _transform;

        public bool IgnoreXMovement { get; set; }
        public bool IgnoreYMovement { get; set; }

        public Transform ObjectToFollow
        {
            get => _objectToFollow;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(
                        $"{value.GetType().FullName} in {GetType().FullName}");

                _objectToFollow = value;
            }
        }

        public FollowerObject(
            Transform objectToFollow,
            bool ignoreXMovement = false,
            bool ignoreYMovement = true,
            float updateIntervalInSeconds = 1f)
        {
            _objectToFollow = objectToFollow;
            IgnoreXMovement = ignoreXMovement;
            IgnoreYMovement = ignoreYMovement;
            _updateIntervalInSeconds = updateIntervalInSeconds;

            _transform = transform;
        }

        public void BeginFollowing()
        {
            Follow().Forget();
        }

        private async UniTask Follow()
        {
            while (this != null)
            {
                Vector3 movedPosition = GetMovedPosition();
                _transform.position = movedPosition;

                await MyUniTask.Delay(_updateIntervalInSeconds);
            }
        }

        private Vector3 GetMovedPosition()
        {
            Vector3 movedPosition = _transform.position;
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