using UnityEngine;

namespace Core.Common
{
    public class GroundChecker
    {
        private readonly Transform _thisTransform;
        private readonly float _distance;
        private readonly LayerMask _groundLayer;

        public GroundChecker(Transform transform, float distance, LayerMask groundLayer)
        {
            _thisTransform = transform;
            _distance = distance;
            _groundLayer = groundLayer;
        }

        public bool HaveGroundBelow()
        {
            return Physics2D.Raycast(
                _thisTransform.position,
                Vector2.down,
                _distance,
                _groundLayer);
        }
    }
}
