using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using NTC.Pool;
using Core.Other;
using UnityTools;
using Core.Factories;

namespace Core.Level
{
    public class LevelGenerator : ILevelGenerator, ILocationsHandler
    {
        private const float HeroPositionCheckFrequency = 1.5f;
        private const int SpecialPlatformSequentialNumber = 4;
        private const int LocationChangeSequentialNumber = 10;

        private readonly LevelGeneratorConfig _config;
        private readonly IPlatformFactory<Platform> _platformFactory;
        private readonly Queue<Platform> _platformsOnLevel = new();
        private readonly BackgroundHandler _backgroundHandler;
        private Transform _centerTransform;
        private int _platformNumber = 0;
        private float _lastGeneratedPlatformX = 0f;
        private int _locationNumber = 0;
        private CancellationTokenSource _cancellationTokenSource;

        public Location CurrentLocation => _config.Locations[_locationNumber];
        private float HeroX => _centerTransform.position.x;

        private bool IsNowSpecialPlatformTurn => 
            _platformNumber % SpecialPlatformSequentialNumber == 0 && _platformNumber > 0;

        private bool IsNowLocationChangeTurn =>
             _platformNumber % LocationChangeSequentialNumber == 0 && _platformNumber > 0;

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
        
        public LevelGenerator(LevelGeneratorConfig config, Transform platfomrsParent)
        {
            _config = config;

            _lastGeneratedPlatformX = _config.XtartPoint;

            ILocationsHandler locationsHandler = this;
            _platformFactory = new PlatformFactory(locationsHandler, platfomrsParent);
            _backgroundHandler = new();
        }

        public void Dispose()
        {
            _backgroundHandler.Dispose();

            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        public void InitializeCenter(Transform transform)
        {
            _centerTransform = transform;
            CheckPlayerPosition().Forget();
        }

        public void Generate()
        {
            SpawnStartPlatform();
            GenerateDefaultAmount();
        }

        public void SpawnStartPlatform()
        {
            Platform platform = _platformFactory.Create(_config.StartPlatform);
            
            Vector2 newPosition = platform.transform.position;
            newPosition.y = _config.PlatformsY;
            platform.transform.position = newPosition;

            UpdateGenerationData(platform);
        }

        public void GenerateDefaultAmount()
        {
            for (var i = 0; i < _config.PlatformsAmountToGenerate; i++)
                GenerateRandomPlatform();
        }

        private async UniTask CheckPlayerPosition()
        {
            _cancellationTokenSource = new();
            CancellationToken token = _cancellationTokenSource.Token;
            
            while (this != null)
            {
                if (token.IsCancellationRequested == true)
                    break;
                
                if (IsLevelMidPointXLessHeroX == true) 
                {
                    DespawnOldestPlatform();
                    GenerateRandomPlatform();
                }

                await MyUniTask
                    .Delay(HeroPositionCheckFrequency, token)
                    .SuppressCancellationThrow();
            }

            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
        }

        private void GenerateRandomPlatform()
        {
            if (IsNowLocationChangeTurn == true)
                ChangeLocation();

            Vector2 position = GetPlatformPosition();
            Platform randomPlatform = IsNowSpecialPlatformTurn == true
                ? GenerateSpecialPlatform(position)
                : GenerateSimplePlatform(position);

            UpdateGenerationData(randomPlatform);
        }

        private void ChangeLocation()
        {
            _locationNumber++;
                
            int locationsLength = _config.Locations.Length;
            if (_locationNumber >= locationsLength)
                _locationNumber = _config.Locations.GetRandomIndex();
        }

        private Platform GenerateSimplePlatform(Vector2 position) =>
            _platformFactory.CreateSimple(position);

        private Platform GenerateSpecialPlatform(Vector2 position) =>
            _platformFactory.CreateSpecial(position);

        private void UpdateGenerationData(Platform platform)
        {
            _lastGeneratedPlatformX += Platform.Length;
            _platformNumber++;
            _platformsOnLevel.Enqueue(platform);
        }

        private void DespawnOldestPlatform() => 
            NightPool.Despawn(_platformsOnLevel.Dequeue());

        private Vector2 GetPlatformPosition() => 
            new(_lastGeneratedPlatformX, _config.PlatformsY);
    }
}