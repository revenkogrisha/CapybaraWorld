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
            SpawnMarker[] filtered = GetFiltered(platform, EntityKind.Food);
            SpawnAllMarkers(filtered, _entityAssets.FoodPrefab, platform.transform);
        }

        private void SpawnChests(Platform platform)
        {
            SpawnMarker[] filtered = GetFiltered(platform, EntityKind.Chest);

            Transform parent = platform.transform;
            SpawnChests(parent, filtered, ChestKind.Simple);
            SpawnChests(parent, filtered, ChestKind.Treasure);
        }

        private void SpawnEnemies(Platform platform)
        {
            SpawnMarker[] filtered = GetFiltered(platform, EntityKind.Enemy);
                
            Transform parent = platform.transform;
            SpawnEnemies(parent, filtered, EnemyKind.Cactopus);
            SpawnEnemies(parent, filtered, EnemyKind.Stoney);
            SpawnEnemies(parent, filtered, EnemyKind.Cactoculus);
        }

        private SpawnMarker[] GetFiltered(Platform platform, EntityKind kind) =>
            SpawnMarker.FilterByKind(platform.SpawnMarkers, kind);

        private void SpawnChests(Transform parent, SpawnMarker[] filtered, ChestKind kind)
        {
            SpawnMarker[] markers = filtered
                .Where(marker => marker.ChestKind == kind)
                .ToArray();

            Chest prefab = _entityAssets.Chests[kind];
            SpawnAllMarkers(markers, prefab, parent);
        }

        private void SpawnEnemies(Transform parent, SpawnMarker[] filtered, EnemyKind kind)
        {
            SpawnMarker[] markers = filtered
                .Where(marker => marker.EnemyKind == kind)
                .ToArray();

            Enemy prefab = _enemyAssets.Enemies[kind];
            SpawnAllMarkers(markers, prefab, parent);
        }

        private void SpawnAllMarkers<TEntity>(SpawnMarker[] markers, 
            TEntity prefab, 
            Transform parent) where TEntity : Entity
        {
            foreach (SpawnMarker marker in markers)
            {
                TEntity entity = NightPool.Spawn(prefab, parent);
                entity.SetLocalScale(prefab.GetLocalScale());

                SetupProduct(entity, marker);
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