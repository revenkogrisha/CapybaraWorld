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

            SpawnSimpleChests(platform, filtered);
            SpawnTreasureChests(platform, filtered);
        }

        private void SpawnEnemies(Platform platform)
        {
            SpawnMarker[] filtered = SpawnMarker.FilterByKind(platform.SpawnMarkers,
                EntityKind.Enemy);
                
            SpawnCactopuses(platform, filtered);
            SpawnStoneys(platform, filtered);
            SpawnCactoculuses(platform, filtered);
        }

        private void SetupProduct(Entity product, SpawnMarker marker)
        {
            Vector3 localPosition = marker.GetLocalPosition();
            product.SetLocalPosition(localPosition);
            marker.SetProduct(product.gameObject);
        }

        private void SpawnSimpleChests(Platform platform, SpawnMarker[] filtered)
        {
            SpawnMarker[] simpleChests = filtered
                .Where(marker => marker.ChestKind == ChestKind.Simple)
                .ToArray();

            Chest simpleChestPrefab = _entityAssets.Chests[ChestKind.Simple];
            foreach (SpawnMarker marker in simpleChests)
            {
                Chest chest = NightPool.Spawn(simpleChestPrefab, platform.transform);
                SetupProduct(chest, marker);
            }
        }

        private void SpawnTreasureChests(Platform platform, SpawnMarker[] filtered)
        {
            SpawnMarker[] treasureChests = filtered
                .Where(marker => marker.ChestKind == ChestKind.Treasure)
                .ToArray();

            Chest treasureChestPrefab = _entityAssets.Chests[ChestKind.Treasure];
            foreach (SpawnMarker marker in treasureChests)
            {
                Chest chest = NightPool.Spawn(treasureChestPrefab, platform.transform);
                SetupProduct(chest, marker);
            }
        }

        private void SpawnCactopuses(Platform platform, SpawnMarker[] filtered)
        {
            SpawnMarker[] cactopusMarkers = filtered
                .Where(marker => marker.EnemyKind == EnemyKind.Cactopus)
                .ToArray();

            Enemy cactopusPrefab = _enemyAssets.Enemies[EnemyKind.Cactopus];
            foreach (SpawnMarker marker in cactopusMarkers)
            {
                Enemy enemy = NightPool.Spawn(cactopusPrefab, platform.transform);
                SetupProduct(enemy, marker);
            }
        }

        private void SpawnStoneys(Platform platform, SpawnMarker[] filtered)
        {
            SpawnMarker[] stoneyMarkers = filtered
                .Where(marker => marker.EnemyKind == EnemyKind.Stoney)
                .ToArray();

            Enemy stoneyPrefab = _enemyAssets.Enemies[EnemyKind.Stoney];
            foreach (SpawnMarker marker in stoneyMarkers)
            {
                Enemy enemy = NightPool.Spawn(stoneyPrefab, platform.transform);
                SetupProduct(enemy, marker);
            }
        }

        private void SpawnCactoculuses(Platform platform, SpawnMarker[] filtered)
        {
            SpawnMarker[] cactoculusMarkers = filtered
                .Where(marker => marker.EnemyKind == EnemyKind.Cactoculus)
                .ToArray();

            Enemy cactoculusPrefab = _enemyAssets.Enemies[EnemyKind.Cactoculus];
            foreach (SpawnMarker marker in cactoculusMarkers)
            {
                Enemy enemy = NightPool.Spawn(cactoculusPrefab, platform.transform);
                enemy.SetLocalScale(cactoculusPrefab.GetLocalScale());
                SetupProduct(enemy, marker);
            }
        }
    }
}