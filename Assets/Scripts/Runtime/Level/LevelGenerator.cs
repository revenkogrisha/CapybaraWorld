using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using System.Threading;
using NTC.Pool;
using Core.Other;
using Core.Player;

namespace Core.Level
{
    public class LevelGenerator : IDisposable
    {
        private const float HeroPositionCheckFrequency = 1.5f;

        private readonly LevelGeneratorConfig _config;
        private readonly Transform _platformsParent;
        private readonly Queue<Platform> _platformsOnLevel = new();
        private readonly Transform _heroTransform;
        private int _platformNumber = 0;
        private float _lastGeneratedPlatformX = 0f;
        private CancellationToken _cancellationToken;

        private string PlatformName => $"Platform №{_platformNumber}";
        private float HeroX => _heroTransform.position.x;

        private bool IsLevelMidPointXLessHeroX
        {
            get
            {
                float midPointX = GetLevelMidPointX();
                return midPointX < HeroX;

                float GetLevelMidPointX()
                {
                    Platform oldestPlatform = _platformsOnLevel.Peek();
                    float oldestPlatformX = oldestPlatform.transform.position.x;
                    return (_lastGeneratedPlatformX + oldestPlatformX) / 2f;
                }
            }
        }

        public LevelGenerator(PlayerTest hero, Transform platformsParent = null)
        {
            _heroTransform = hero.transform;
            _platformsParent = platformsParent;
            //_cancellationToken = this.GetCancellationTokenOnDestroy();

            _lastGeneratedPlatformX = _config.XStartPoint;
        }


        public void Dispose()
        {
            // Checkout when does cancel. token is disposed
        }

        public void Generate()
        {
            GenerateStartPlatform();
            GenerateDefaultAmount();
        }

        private void GenerateStartPlatform()
        {
            GeneratePlatform(_config.StartPlatform);
        }
        
        private void GenerateDefaultAmount()
        {
            for (var i = 0; i < _config.PlatformsAmountToGenerate; i++)
                GenerateRandomPlatform();
        }

        public void StartPlayerPositionChecking()
        {
            CheckPlayerPosition().Forget();
        }

        private async UniTask CheckPlayerPosition()
        {
            while (this != null)
            {
                await UniTask.WaitUntil(() => _heroTransform != null);

                if (IsLevelMidPointXLessHeroX == true) 
                {
                    DespawnOldestPlatform();
                    GenerateRandomPlatform();
                }

                await MyUniTask.Delay(HeroPositionCheckFrequency);
            }
        }

        private void GenerateRandomPlatform()
        {
            Platform randomPlatform = GetRandomPlatform(_config.Platforms);
            GeneratePlatform(randomPlatform);
        }

        private T GetRandomPlatform<T>(T[] platformPrefabs)
            where T : Platform
        {
            int random = Random.Range(0, platformPrefabs.Length);
            return platformPrefabs[random];
        }

        private void GeneratePlatform(Platform platformPrefab)
        {
            Platform platform = SpawnPlatform(platformPrefab);
            platform.transform.SetParent(_platformsParent);
            platform.name = PlatformName;

            _lastGeneratedPlatformX += Platform.Length;
            _platformNumber++;
            _platformsOnLevel.Enqueue(platform);
        }

        private Platform SpawnPlatform(Platform platformPrefab)
        {
            var position = new Vector3(_lastGeneratedPlatformX, _config.PlatformsY);
            Platform platform = NightPool.Spawn(
                platformPrefab,
                position,
                Quaternion.identity);
            
            return platform;
        }

        private void DespawnOldestPlatform()
        {
            Platform oldestPlatformInGame = _platformsOnLevel.Dequeue();
            NightPool.Despawn(oldestPlatformInGame);
        }
    }
}