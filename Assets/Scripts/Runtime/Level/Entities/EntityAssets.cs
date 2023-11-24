using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "Entity Assets", menuName = "Collections/Entity Assets")]
    public class EntityAssets : ScriptableObject
    {
        [Title("Food")]
        [SerializeField] private Food _foodPrefab;

        [Title("Chests")]
        [SerializeField] private Chest _simpleChestPrefab;
        [SerializeField] private Chest _treasureChestPrefab;

        public readonly Dictionary<ChestKind, Chest> Chests = new();

        public Food FoodPrefab => _foodPrefab;

        private void OnEnable()
        {
            Chests[ChestKind.Simple] = _simpleChestPrefab;
            Chests[ChestKind.Treasure] = _treasureChestPrefab;
        }
    }
}
