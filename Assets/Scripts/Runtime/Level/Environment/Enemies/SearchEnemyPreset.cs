using UnityEngine;

namespace Core.Level
{
    public class SearchEnemyPreset : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _spotRadius = 10f;
        [SerializeField, Min(0f)] private float _spotInterval = 0.8f;
        [SerializeField] private LayerMask _targetLayer;

        public float SpotRadius => _spotRadius;
        public float SpotInterval => _spotInterval;
        public LayerMask TargetLayer => _targetLayer;
    }
}