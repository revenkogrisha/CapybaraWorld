using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using NTC.Pool;
using Core.Other;
using Core.Player;
using UnityTools;
using Core.Factories;

namespace Core.Level
{
    public class LevelGenerator : ILocationsHandler
    {
        private const float HeroPositionCheckFrequency = 1.5f;
        private const int SpecialPlatformSequentialNumber = 4;
        private const int LocationChangeSequentialNumber = 10;
        private const float BackgroundLerpDuration = 3.5f;

        private readonly LevelGeneratorConfig _config;
        private readonly IPlatformFactory<Platform> _platformFactory;
        private readonly Queue<Platform> _platformsOnLevel = new();
        private Camera _camera;
        private Transform _heroTransform;
        private int _platformNumber = 0;
        private float _lastGeneratedPlatformX = 0f;
        private int _locationNumber = 0;
        private Color _defaultBackground;

        public Location CurrentLocation => _config.Locations[_locationNumber];
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
        
        public LevelGenerator(LevelGeneratorConfig config, PlayerTest hero)
        {
            _config = config;

            _heroTransform = hero.transform;
            _lastGeneratedPlatformX = _config.XtartPoint;
            _camera = Camera.main;

            _defaultBackground = _camera.backgroundColor;

            ILocationsHandler locationsHandler = this;
            Transform parent = _config.PlatformsParent;
            _platformFactory = new PlatformFactory(locationsHandler, parent);
        }

        private void Initialize()
        {
            //                                                          Probably need to separate
            ChangeBackgroundColor();
            CheckPlayerPosition().Forget();
        }

        public void GenerateLevel()
        {
            SpawnStartPlatform();
            GenerateDefaultAmount();
        }

        public void SpawnStartPlatform() =>
            _platformFactory.Create(_config.StartPlatform);

        public void GenerateDefaultAmount()
        {
            for (var i = 0; i < _config.PlatformsAmountToGenerate; i++)
                GenerateRandomPlatform();
        }

        private async UniTask CheckPlayerPosition()
        {
            while (this != null)
            {
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

        private void ChangeBackgroundColor()
        {
            Location currentLocation = CurrentLocation;
            Color newBackground;
            if (currentLocation.UseDefaultBackground == true)
                newBackground = _defaultBackground;
            else
                newBackground = currentLocation.BackgroundColor;

            LerpBackground(newBackground).Forget();
        }

        private async UniTask LerpBackground(Color newBackground, CancellationToken token = default)
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

        private void DespawnOldestPlatform()
        {
            Platform oldestPlatformInGame = _platformsOnLevel.Dequeue();
            NightPool.Despawn(oldestPlatformInGame);
        }

        private Vector2 GetPlatformPosition() => 
            new(_lastGeneratedPlatformX, _config.PlatformsY);
    }
}