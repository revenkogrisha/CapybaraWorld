using Core.Level;
using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Player Assets", menuName = "Collections/Player Assets")]
    public class PlayerAssets : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private Hero _playerPrefab;
        [SerializeField] private DeadlyForPlayerObject _playerDeadlinePrefab;

        [Header("Spawn Settings")]
        [SerializeField] private Vector2 _playerSpawnPosition;
        [SerializeField] private Vector2 _playerDeadlinePosition;

        public Hero PlayerPrefab => _playerPrefab;
        public DeadlyForPlayerObject PlayerDeadlinePrefab => _playerDeadlinePrefab;
        public Vector2 PlayerSpawnPosition => _playerSpawnPosition;
        public Vector2 PlayerDeadlinePosition => _playerDeadlinePosition;
    }
}
