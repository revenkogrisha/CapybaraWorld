using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "Enemy Config", menuName = "Configs/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _spotRadius = 20f;
        [SerializeField, Min(0f)] private float _spotInterval = 0.8f;
        [SerializeField, Min(0f)] private float _speed = 8f;
        [SerializeField] private LayerMask _targetLayer;

        public float SpotRadius => _spotRadius;
        public float SpotInterval => _spotInterval;
        public float Speed => _speed;
        public LayerMask TargetLayer => _targetLayer;
    }
}