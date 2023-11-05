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

        [Header("Hero Settings")]
        [SerializeField, Range(0f, 100f)] private float _grappleRadius = 10f;
        [SerializeField] private float _grappleJumpVelocityMultiplier = 1.5f;
        [SerializeField, Range(0f, 100f)] private float _runSpeed = 30f;
        [SerializeField, Range(0f, 1f)] private float _accelerationTime = 0.3f;

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
        public LayerMask GroundLayer => _groundLayer;
        public LayerMask JointLayer => _jointLayer;
        public Vector2 PlayerSpawnPosition => _playerSpawnPosition;
        public Vector2 PlayerDeadlinePosition => _playerDeadlinePosition;
    }
}
