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
            SpawnMarker[] filtered = SpawnMarker.FilterByKind(platform.SpawnMarkers, 
                EntityKind.Food);
                
            foreach (SpawnMarker marker in filtered)
            {
                Food food = NightPool.Spawn(_entityAssets.FoodPrefab, platform.transform);
                SetupProduct(food, marker);
            }
        }

        private void SpawnChests(Platform platform)
        {
            SpawnMarker[] filtered = SpawnMarker.FilterByKind(platform.SpawnMarkers,
                EntityKind.Chest);

            SpawnChests(platform, filtered, ChestKind.Simple);
            SpawnChests(platform, filtered, ChestKind.Treasure);
        }

        private void SpawnEnemies(Platform platform)
        {
            SpawnMarker[] filtered = SpawnMarker.FilterByKind(platform.SpawnMarkers,
                EntityKind.Enemy);
                
            SpawnEnemies(platform, filtered, EnemyKind.Cactopus);
            SpawnEnemies(platform, filtered, EnemyKind.Stoney);
            SpawnEnemies(platform, filtered, EnemyKind.Cactoculus);
        }

        private void SetupProduct(Entity product, SpawnMarker marker)
        {
            Vector3 localPosition = marker.GetLocalPosition();
            product.SetLocalPosition(localPosition);
            marker.SetProduct(product.gameObject);
        }

        private void SpawnChests(Platform platform, SpawnMarker[] filtered, ChestKind kind)
        {
            SpawnMarker[] markers = filtered
                .Where(marker => marker.ChestKind == ChestKind.Simple)
                .ToArray();

            Chest prefab = _entityAssets.Chests[kind];
            foreach (SpawnMarker marker in markers)
            {
                Chest chest = NightPool.Spawn(prefab, platform.transform);
                SetupProduct(chest, marker);
            }
        }

        private void SpawnEnemies(Platform platform, SpawnMarker[] filtered, EnemyKind kind)
        {
            SpawnMarker[] markers = filtered
                .Where(marker => marker.EnemyKind == kind)
                .ToArray();

            Enemy prefab = _enemyAssets.Enemies[kind];
            foreach (SpawnMarker marker in markers)
            {
                Enemy enemy = NightPool.Spawn(prefab, platform.transform);
                enemy.SetLocalScale(prefab.GetLocalScale());
                SetupProduct(enemy, marker);
            }
        }
    }
}