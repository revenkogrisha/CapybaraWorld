using Core.Common;
using TriInspector;
using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Hero Config", menuName = "Configs/Hero Config")]
    public class HeroConfig : ScriptableObject
    {
        [Title("Grappling Settings")]
        [SerializeField, Range(8f, 15f)] private float _grappleRadius = 11f;
        [SerializeField, Min(0f)] private float _releaseVelocityMultiplier = 1.4f;

        [Space]
        [SerializeField, Min(0f)] private float _onGrappledVelocityMultiplier = 0.7f;
        [SerializeField] private Vector2 _onGrappledVelocityVector = new(1f, 0.8f);

        [Title("Run Settings")]
        [SerializeField, Range(0f, 30f)] private float _runSpeed = 12f;
        [SerializeField, Range(0f, 1f)] private float _accelerationTime = 0.15f;

        [Title("Dash Settings")]
        [SerializeField, Min(0f)] private float _dashForce = 30f;
        [SerializeField, Min(0f)] private float _dashDuration = 0.2f;
        [SerializeField, Min(0f)] private float _dashCooldown = 2f;

        [Space]
        [SerializeField, Min(0f)] private float _defaultGravityScale = 1.9f;
        [SerializeField, Min(0f)] private float _dashGravityScale = 0f;

        [Title("Jump Settings")]
        [SerializeField] private AnimationCurve _jumpProgression;
        [SerializeField] private Vector2 _jumpVector = new(0.2f, 1f);
        [SerializeField, Min(0f)] private float _jumpDuration = 0.55f;
        [SerializeField, Min(0f)] private float _jumpForce = 18f;
        [SerializeField, Min(0f)] private float _descendDuration = 1f;

        [Space]
        [SerializeField] private ParticlesName _jumpParticlesName = ParticlesName.Dust;

        [Title("Collisions")]
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
        
        public float DefaultGravityScale => _defaultGravityScale;
        public float DashGravityScale => _dashGravityScale;
        
        public AnimationCurve JumpProgression => _jumpProgression;
        public Vector2 JumpVector => _jumpVector;
        public float JumpDuration => _jumpDuration;
        public float JumpForce => _jumpForce;
        public float DescendDuration => _descendDuration;

        public ParticlesName JumpParticlesName => _jumpParticlesName;
        
        public LayerMask GroundLayer => _groundLayer;
        public LayerMask JointLayer => _jointLayer;

    }
}