using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using NTC.Pool;
using Core.Other;
using UnityTools;
using Core.Factories;
using Zenject;
using Core.Saving;
using System;
using Core.Editor.Debugger;
using Core.UI;

namespace Core.Level
{
    public class LevelGenerator : ILevelGenerator, ILocationsHandler
    {
        private const float PositionCheckInterval = 1.5f;

        private readonly Queue<Platform> _platformsOnLevel;
        private readonly AreaLabelsCollection _areaLabels;
        private readonly LevelGeneratorConfig _config;
        private readonly EntitySpawner _entitySpawner;
        private readonly IPlatformFactory<Platform> _platformFactory;
        private readonly BackgroundHandler _backgroundHandler;
        private AreaLabelsService _areaLabelsService;
        private AreaLabelContainer _labelContainer;
        private CancellationTokenSource _cts;
        private Transform _centerTransform;
        private int _platformNumber = 0;
        private float _lastGeneratedPlatformX = 0f;
        private Location _currentLocation;
        private int _locationIndex = 0;

        public Location CurrentLocation => _currentLocation;
        private float HeroX => _centerTransform.position.x;
        private bool IsLevelMidPointXLessHeroX => GetLevelMidPointX() < HeroX;
        private bool IsNowSpecialPlatformTurn => 
            _platformNumber % _config.SpecialPlatformSequentialNumber == 0 && _platformNumber > 0;

        [Inject]
        public LevelGenerator(
            LevelGeneratorConfig config,
            AreaLabelsCollection areaLabels,
            Transform platfomrsParent,
            EntitySpawner entitySpawner)
        {
            _config = config;
            _entitySpawner = entitySpawner;
            _lastGeneratedPlatformX = _config.XtartPoint;
            _platformsOnLevel = new();

            _areaLabels = areaLabels;

            ILocationsHandler locationsHandler = this;
            _platformFactory = new PlatformFactory(locationsHandler, platfomrsParent);
            _backgroundHandler = new(_config.BackgroundPrefab);
        }

        public void Dispose()
        {
            _cts.Clear();
            _areaLabelsService.Dispose();
            _labelContainer.gameObject.SelfDestroy();
        }

        public void InitializeLabels(AreaLabelContainer container)
        {
            _areaLabelsService = new(_areaLabels, container);
            _labelContainer = container;
        }

        public void Initialize(Transform transform)
        {
            _centerTransform = transform;
            CheckCenterPosition().Forget();
        }

        public void Save(SaveData data) => 
            data.LocationIndex = _locationIndex;

        public void Load(SaveData data) => 
            SetLocationByIndex(data.LocationIndex);

        public void SetRandomLocation()
        {
            int index = _config.Locations.GetRandomIndex();
            Location location = _config.Locations[index];

            _currentLocation = location;
            _locationIndex = index;
        }

        public void Generate()
        {
            SpawnStartPlatform();
            GenerateDefaultAmount();

            CreateBackground();
        }

        public void Clean()
        {
            _platformNumber = 0;
            _lastGeneratedPlatformX = 0f;

            foreach (Platform platform in _platformsOnLevel)
                NightPool.Despawn(platform);
                
            _platformsOnLevel.Clear();
            _backgroundHandler.Dispose();
        }

        public void SpawnStartPlatform()
        {
            Platform platform = _platformFactory.Create(_config.StartPlatform);
            
            Vector2 newPosition = platform.GetPosition();
            newPosition.y = _config.PlatformsY;
            platform.SetPosition(newPosition);

            UpdateGenerationData(platform);
        }

        public void GenerateDefaultAmount()
        {
            for (var i = 0; i < _config.PlatformsStartAmount; i++)
                GenerateRandomPlatform();
        }

        private void CreateBackground()
        {
            Vector2 position = _config.BackgroundPosition;
            BackgroundPreset preset = CurrentLocation.BackgroundPreset;
            _backgroundHandler.CreateBackground(position, preset);
        }

        private async UniTaskVoid CheckCenterPosition()
        {
            _cts = new();
            CancellationToken token = _cts.Token;

            try
            {
                while (true)
                {
                    if (IsLevelMidPointXLessHeroX == true)
                    {
                        DespawnOldestPlatform();
                        GenerateRandomPlatform();
                    }

                    float positionX = _centerTransform.position.x;
                    int landPlatformNumber = _config.SpecialPlatformSequentialNumber;
                    _areaLabelsService.CheckDistance(positionX, landPlatformNumber);

                    await UniTaskUtility.Delay(PositionCheckInterval, token);
                }
            }
            catch (OperationCanceledException) {  }
            catch (Exception ex)
            {
                RDebug.Warning($"{nameof(LevelGenerator)}::{nameof(CheckCenterPosition)}: {ex.Message} \n{ex.StackTrace}");
            }
            finally
            {
                _cts.Clear();
                _cts = null;
            }
        }

        private void SetLocationByIndex(int index)
        {
            if (index >= _config.Locations.Length)
                throw new ArgumentOutOfRangeException("Incorrect location index was set!");

            if (index < 0)
                throw new ArgumentException("Location index cannot be lower than zero!");

            Location location = _config.Locations[index];
            if (location == null)
                throw new ArgumentNullException("Location gotten by index is null!");

            _currentLocation = location;
        }

        private void GenerateRandomPlatform()
        {
            Vector2 position = GetPlatformPosition();
            Platform randomPlatform = IsNowSpecialPlatformTurn == true
                ? GenerateSpecialPlatform(position)
                : GenerateSimplePlatform(position);

            UpdateGenerationData(randomPlatform);
        }

        private Platform GenerateSimplePlatform(Vector2 position)
        {
            Platform platform = _platformFactory.CreateSimple(position);
            _entitySpawner.SpawnEntities(platform);
            return platform;
        }


        private Platform GenerateSpecialPlatform(Vector2 position)
        {
            Platform platform = _platformFactory.CreateSpecial(position);
            _entitySpawner.SpawnEntities(platform);
            return platform;
        }


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

        private float GetLevelMidPointX()
        {
            Platform oldestPlatform = _platformsOnLevel.Peek();
            float oldestPlatformX = oldestPlatform.GetPosition().x;
            return (_lastGeneratedPlatformX + oldestPlatformX) / 2f;
        }
    }
}