using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Cysharp.Threading.Tasks;
using System.Threading;
using NTC.Pool;
using Core.Other;
using Core.Player;
using UnityTools;

namespace Core.Level
{
    public class LevelGenerator
    {
        private const float HeroPositionCheckFrequency = 1.5f;
        private const int SpecialPlatformSequentialNumber = 4;
        private const int LocationChangeSequentialNumber = 10;
        private const float BackgroundLerpDuration = 3.5f;

        private readonly LevelGeneratorConfig _config;

        private readonly Queue<Platform> _platformsOnLevel = new();
        private Camera _camera;
        private Transform _heroTransform;
        private int _platformNumber = 0;
        private float _lastGeneratedPlatformX = 0f;
        private int _locationNumber = 0;
        private Color _defaultBackground;
        private CancellationToken _cancellationToken;

        private string PlatformName => $"Platform №{_platformNumber}";
        private Location CurrentLocation => _config.Locations[_locationNumber];
        private float HeroX => _heroTransform.position.x;

        private bool IsNowSpecialPlatformTurn 
        {
            get
            {
                return _platformNumber % SpecialPlatformSequentialNumber == 0
                && _platformNumber > 0;
            }
        }

        private bool IsNowLocationChangeTurn 
        {
            get
            {
                return _platformNumber % LocationChangeSequentialNumber == 0
                && _platformNumber > 0;
            }
        }

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

        public event Action<float> OnHeroXPositionUpdated;
        
        public LevelGenerator(PlayerTest hero)
        {
            _heroTransform = hero.transform;
            _lastGeneratedPlatformX = _config.XtartPoint;
            _camera = Camera.main;
            //_cancellationToken = this.GetCancellationTokenOnDestroy();

            _defaultBackground = _camera.backgroundColor;
        }

        private void Initialize()
        {
            ChangeBackgroundColor();
            CheckPlayerPosition().Forget();
        }

        public void SpawnStartPlatform()
        {
            GeneratePlatform(_config.StartPlatform);
        }
        
        public void GenerateDefaultAmount()
        {
            for (var i = 0; i < _config.PlatformsAmountToGenerate; i++)
                GenerateRandomPlatform();
        }

        private async UniTask CheckPlayerPosition()
        {
            while (this != null)
            {
                OnHeroXPositionUpdated?.Invoke(HeroX);

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
            if (IsNowLocationChangeTurn == true)
            {
                ChangeLocation();
                ChangeBackgroundColor();
            }
 
            Location currentLocation = CurrentLocation;
            SimplePlatform[] simplePlatforms = currentLocation.SimplePlatforms;
            SpecialPlatform[] specialPlatforms = currentLocation.SpecialPlatforms;

            Platform randomPlatform;
            randomPlatform = IsNowSpecialPlatformTurn
                ? GetRandomPlatform(specialPlatforms)
                : GetRandomPlatform(simplePlatforms);

            GeneratePlatform(randomPlatform);
        }

        private void ChangeLocation()
        {
            _locationNumber++;
                
            int locationsLength = _config.Locations.Length;
            if (_locationNumber >= locationsLength)
                _locationNumber = _config.Locations.GetRandomIndex();
        }

        private void ChangeBackgroundColor()
        {
            Location currentLocation = CurrentLocation;
            Color newBackground;
            if (currentLocation.UseDefaultBackground == true)
                newBackground = _defaultBackground;
            else
                newBackground = currentLocation.BackgroundColor;

            LerpBackground(newBackground, _cancellationToken).Forget();
        }

        private async UniTask LerpBackground(Color newBackground, CancellationToken token)
        {
            float elapsedTime = 0;
            Color currentBackground = _camera.backgroundColor;
            while (elapsedTime < BackgroundLerpDuration)
            {
                float time = elapsedTime / BackgroundLerpDuration;
                _camera.backgroundColor = Color.Lerp(currentBackground, newBackground, time);

                elapsedTime += Time.deltaTime;
                await UniTask.NextFrame(token);
            }
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
            platform.transform.SetParent(_config.PlatformsParent);
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