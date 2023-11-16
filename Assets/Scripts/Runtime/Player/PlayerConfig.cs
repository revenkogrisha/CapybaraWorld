using Core.Level;
using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private Hero _playerPrefab;
        [SerializeField] private DeadlyForPlayerObject _playerDeadlinePrefab;
        [SerializeField] private MiddleObject _middleObjectPrefab;

        [Header("Hero Grappling Settings")]
        [SerializeField, Range(0f, 100f)] private float _grappleRadius = 10f;
        [SerializeField, Min(0f)] private float _grappleJumpVelocityMultiplier = 1.5f;

        [Header("Hero Run Settings")]
        [SerializeField, Range(0f, 100f)] private float _runSpeed = 15f;
        [SerializeField, Range(0f, 1f)] private float _accelerationTime = 0.3f;

        [Header("Hero Dash Settings")]
        [SerializeField, Min(0f)] private float _dashForce = 30f;
        [SerializeField, Min(0f)] private float _dashDuration = 0.2f;
        [SerializeField, Min(0f)] private float _dashCooldown = 2f;

        [Header("Hero Jump Settings")]
        [SerializeField] private AnimationCurve _jumpProgression;
        [SerializeField] private Vector2 _jumpVector = new(0.2f, 1f);
        [SerializeField, Min(0f)] private float _jumpDuration = 0.8f;
        [SerializeField, Min(0f)] private float _jumpForce = 20f;

        [Header("Hero Collisions")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private LayerMask _jointLayer;

        [Header("Spawn Settings")]
        [SerializeField] private Vector2 _playerSpawnPosition;
        [SerializeField] private Vector2 _playerDeadlinePosition;

        public Hero PlayerPrefab => _playerPrefab;
        public DeadlyForPlayerObject PlayerDeadlinePrefab => _playerDeadlinePrefab;
        public MiddleObject MiddleObjectPrefab => _middleObjectPrefab;
        public float GrappleRadius => _grappleRadius;
        public float GrappleJumpVelocityMultiplier => _grappleJumpVelocityMultiplier;
        public float RunSpeed => _runSpeed;
        public float AccelerationTime => _accelerationTime;
        public float DashForce => _dashForce;
        public float DashDuration => _dashDuration;
        public float DashCooldown => _dashCooldown;
        public AnimationCurve JumpProgression => _jumpProgression;
        public Vector2 JumpVector => _jumpVector;
        public float JumpDuration => _jumpDuration;
        public float JumpForce => _jumpForce;
        public LayerMask GroundLayer => _groundLayer;
        public LayerMask JointLayer => _jointLayer;
        public Vector2 PlayerSpawnPosition => _playerSpawnPosition;
        public Vector2 PlayerDeadlinePosition => _playerDeadlinePosition;
    }
}