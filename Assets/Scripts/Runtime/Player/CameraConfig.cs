using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Camera Config", menuName = "Configs/Camera Config")]
    public class CameraConfig : ScriptableObject
    {
        [Header("Fov")]
        [SerializeField, Min(0f)] private float _regularFov = 7f;
        [SerializeField, Min(0f)] private float _focusFov = 10f;
        [SerializeField, Range(0f, 1f)] private float _fovChangeDuration = 0.3f;

        [Header("Hit Shake")]
        [SerializeField] private float _hitShakeIntensity = 2f;
        [SerializeField] private float _hitShakeDuration = 0.2f; 

        public float RegularFov => _regularFov;
        public float FocusFov => _focusFov;
        public float FovChangeDuration => _fovChangeDuration;
        public float HitShakeIntensity => _hitShakeIntensity;
        public float HitShakeDuration => _hitShakeDuration;
    }
}
