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
        [SerializeField] private LayerMask _jointLayer;

        [Header("Spawn Settings")]
        [SerializeField] private Vector2 _playerSpawnPosition;
        [SerializeField] private Vector2 _playerDeadlinePosition;

        public Hero PlayerPrefab => _playerPrefab;
        public DeadlyForPlayerObject PlayerDeadlinePrefab => _playerDeadlinePrefab;
        public MiddleObject MiddleObjectPrefab => _middleObjectPrefab;
        public float GrappleRadius => _grappleRadius;
        public float GrappleJumpVelocityMultiplier => _grappleJumpVelocityMultiplier;
        public LayerMask JointLayer => _jointLayer;
        public Vector2 PlayerSpawnPosition => _playerSpawnPosition;
        public Vector2 PlayerDeadlinePosition => _playerDeadlinePosition;
    }
}
