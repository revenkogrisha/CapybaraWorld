using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Hero Animator Config", menuName = "Configs/Hero Animator Config")]
    public class HeroAnimatorConfig : ScriptableObject
    {
        [Header("Body")]
        [SerializeField] private bool _rotateBody = true;
        [SerializeField] private float _bodyRotationSpeed = 2f;
        
        [Header("Arm with Hook")]
        [SerializeField] private bool _rotateArmWithHook = true;
        [SerializeField] private float _handRotationSpeed = 5f;

        
        [Header("Default State")]
        [SerializeField] private Quaternion _defaultRotation = Quaternion.identity;
        [SerializeField] private float _rotateToDefaultDuration = 0.8f;

        public bool RotateBody => _rotateBody;
        public bool RotateArmWithHook => _rotateArmWithHook;
        public float HandRotationSpeed => _handRotationSpeed;
        public float BodyRotationSpeed => _bodyRotationSpeed;
        public Quaternion DefaultRotation => _defaultRotation;
        public float RotateToDefaultDuration => _rotateToDefaultDuration;
    }
}
