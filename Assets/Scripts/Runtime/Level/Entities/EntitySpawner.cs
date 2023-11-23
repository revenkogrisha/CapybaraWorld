using System.Linq;
using Core.Other;
using NTC.Pool;
using UnityEngine;
using Zenject;

namespace Core.Level
{
    public class EntitySpawner
    {
        private readonly EntityAssets _entityAssets;
        private readonly EnemyAssets _enemyAssets;

        [Inject]
        public EntitySpawner(
            EntityAssets entityAssets,
            EnemyAssets enemyAssets)
        {
            _entityAssets = entityAssets;
            _enemyAssets = enemyAssets;
        }

        public void SpawnEntities(Platform platform)
        {
            SpawnFood(platform);
            SpawnChests(platform);
            SpawnEnemies(platform);
        }

        private void SpawnFood(Platform platform)
        {
            SpawnMarker[] markers = platform.SpawnMarkers;
            Transform root = platform.transform;

            SpawnMarker[] filtered = SpawnMarker.FilterByKind(markers, EntityKind.Food);
            foreach (SpawnMarker marker in filtered)
            {
                Food food = NightPool.Spawn(_entityAssets.FoodPrefab, root);
                SetupProduct(food, marker);
            }
        }

        private void SpawnChests(Platform platform)
        {
            SpawnMarker[] markers = platform.SpawnMarkers;
            Transform root = platform.transform;

            SpawnMarker[] filtered = SpawnMarker.FilterByKind(markers, EntityKind.Chest);
            SpawnMarker[] simpleChests = filtered
                .Where(marker => marker.ChestKind == ChestKind.Simple)
                .ToArray();

            SpawnMarker[] treasureChests = filtered
                .Where(marker => marker.ChestKind == ChestKind.Treasure)
                .ToArray();

            Chest simpleChestPrefab = _entityAssets.Chests[ChestKind.Simple];
            foreach (SpawnMarker marker in simpleChests)
            {
                Chest chest = NightPool.Spawn(simpleChestPrefab, root);
                SetupProduct(chest, marker);
            }

            Chest treasureChestPrefab = _entityAssets.Chests[ChestKind.Treasure];
            foreach (SpawnMarker marker in treasureChests)
            {
                Chest chest = NightPool.Spawn(treasureChestPrefab, root);
                SetupProduct(chest, marker);
            }
        }

        private void SpawnEnemies(Platform platform)
        {
            SpawnMarker[] markers = platform.SpawnMarkers;
            Transform root = platform.transform;

            SpawnMarker[] filtered = SpawnMarker.FilterByKind(markers, EntityKind.Enemy);
            SpawnMarker[] cactopusMarkers = filtered
                .Where(marker => marker.EnemyKind == EnemyKind.Cactopus)
                .ToArray();

            Enemy cactopusPrefab = _enemyAssets.Enemies[EnemyKind.Cactopus];
            foreach (SpawnMarker marker in cactopusMarkers)
            {
                Enemy enemy = NightPool.Spawn(cactopusPrefab, root);
                SetupProduct(enemy, marker);
            }
        }

        private void SetupProduct(Entity product, SpawnMarker marker)
        {
            Vector3 localPosition = marker.GetLocalPosition();
            product.SetLocalPosition(localPosition);
            marker.SetProduct(product.gameObject);
        }
    }
}