using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "New Chase Enemy", menuName = "Presets/Chase Enemy")]
    public class ChaseEnemyPreset : SearchEnemyPreset
    {
        [Header("Chase Configuration")]
        [SerializeField, Min(0f)] private float _speed = 8f;
        public float Speed => _speed;
    }
}
