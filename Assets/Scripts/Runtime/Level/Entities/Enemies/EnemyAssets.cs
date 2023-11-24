using System.Collections.Generic;
using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "Enemy Assets", menuName = "Collections/Enemy Assets")]
    public class EnemyAssets : ScriptableObject
    {
        [SerializeField] private Enemy _cactopusPrefab;

        public readonly Dictionary<EnemyKind, Enemy> Enemies = new();

        private void OnEnable()
        {
            Enemies[EnemyKind.Cactopus] = _cactopusPrefab;
        }
    }
}