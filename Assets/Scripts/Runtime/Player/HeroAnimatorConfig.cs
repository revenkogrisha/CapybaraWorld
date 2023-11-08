using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Hero Animator Config", menuName = "Configs/Hero Animator Config")]
    public class HeroAnimatorConfig : ScriptableObject
    {
        [Header("Arm with Hook")]
        [SerializeField] private bool _rotateArmWithHook = true;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private Quaternion _defaultRotation = Quaternion.identity;
        [SerializeField] private float _rotateToDefaultDuration = 0.8f;

        public bool RotateArmWithHook => _rotateArmWithHook;
        public float RotationSpeed => _rotationSpeed;
        public Quaternion DefaultRotation => _defaultRotation;
        public float RotateToDefaultDuration => _rotateToDefaultDuration;
    }
}
