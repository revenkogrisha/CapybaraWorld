using Core.Level;
using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Player Config", menuName = "Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private PlayerTest _playerPrefab;
        [SerializeField] private FollowerObject _playerDeadlinePrefab;
        [SerializeField] private MiddleObject _middleObjectPrefab;

        [Header("Spawn Settings")]
        [SerializeField] private Vector2 _playerSpawnPosition;
        [SerializeField] private Vector2 _playerDeadlinePosition;

        public PlayerTest PlayerPrefab => _playerPrefab;
        public FollowerObject PlayerDeadlinePrefab => _playerDeadlinePrefab;
        public MiddleObject MiddleObjectPrefab => _middleObjectPrefab;
        public Vector2 PlayerSpawnPosition => _playerSpawnPosition;
        public Vector2 PlayerDeadlinePosition => _playerDeadlinePosition;
    }
}
