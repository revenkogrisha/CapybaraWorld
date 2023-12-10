using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Hero Config", menuName = "Configs/Hero Config")]
    public class HeroConfig : ScriptableObject
    {
        [Header("Grappling Settings")]
        [SerializeField, Range(0f, 20f)] private float _grappleRadius = 11f;
        [SerializeField, Min(0f)] private float _releaseVelocityMultiplier = 1.4f;

        [Space]
        [SerializeField, Min(0f)] private float _onGrappledVelocityMultiplier = 0.7f;
        [SerializeField] private Vector2 _onGrappledVelocityVector = new(1f, 0.8f);

        [Header("Run Settings")]
        [SerializeField, Range(0f, 100f)] private float _runSpeed = 12f;
        [SerializeField, Range(0f, 1f)] private float _accelerationTime = 0.15f;

        [Header("Dash Settings")]
        [SerializeField, Min(0f)] private float _dashForce = 30f;
        [SerializeField, Min(0f)] private float _dashDuration = 0.2f;
        [SerializeField, Min(0f)] private float _dashCooldown = 2f;

        [Header("Jump Settings")]
        [SerializeField] private AnimationCurve _jumpProgression;
        [SerializeField] private Vector2 _jumpVector = new(0.2f, 1f);
        [SerializeField, Min(0f)] private float _jumpDuration = 0.55f;
        [SerializeField, Min(0f)] private float _jumpForce = 18f;
        [SerializeField, Min(0f)] private float _descendDuration = 1f;

        [Header("Collisions")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _jointLayer;

        public float GrappleRadius => _grappleRadius;
        public float ReleaseVelocityMultiplier => _releaseVelocityMultiplier;
        public float OnGrappledVelocityMultiplier => _onGrappledVelocityMultiplier;
        public Vector2 OnGrappledVelocityVector => _onGrappledVelocityVector;
        public float RunSpeed => _runSpeed;
        public float AccelerationTime => _accelerationTime;
        public float DashForce => _dashForce;
        public float DashDuration => _dashDuration;
        public float DashCooldown => _dashCooldown;
        public AnimationCurve JumpProgression => _jumpProgression;
        public Vector2 JumpVector => _jumpVector;
        public float JumpDuration => _jumpDuration;
        public float JumpForce => _jumpForce;
        public float DescendDuration => _descendDuration;
        public LayerMask GroundLayer => _groundLayer;
        public LayerMask JointLayer => _jointLayer;
    }
}