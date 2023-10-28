using Core.Level;
using NTC.Pool;
using UnityEngine;

namespace Core.Factories
{
    public class PlatformFactory : IPlatformFactory<Platform>
    {
        private const string PlatformName = "Platform";

        private readonly Transform _platformsParent;
        private readonly ILocationsHandler _locationsHandler;


        public PlatformFactory(ILocationsHandler locationsHandler, Transform platformsParent)
        {
            _locationsHandler = locationsHandler;
            _platformsParent = platformsParent;
        }

        public Platform CreateSimple(Vector2 position)
        {
            Location currentLocation = _locationsHandler.CurrentLocation;
            Platform prefab = GetRandomPlatform(currentLocation.SimplePlatforms);

            Platform platform = Create(prefab);
            platform.transform.position = position;

            return platform;
        }

        public Platform CreateSpecial(Vector2 position)
        {
            Location currentLocation = _locationsHandler.CurrentLocation;
            Platform prefab = GetRandomPlatform(currentLocation.SpecialPlatforms);

            Platform platform = Create(prefab);
            platform.transform.position = position;

            return platform;
        }

        public Platform Create(Platform prefab)
        {
            var position = new Vector2();
            Platform platform = NightPool.Spawn(
                prefab,
                position,
                Quaternion.identity);

            platform.transform.SetParent(_platformsParent);
            platform.name = PlatformName;

            return platform;
        }

        private T GetRandomPlatform<T>(T[] platformPrefabs)
            where T : Platform
        {
            int random = Random.Range(0, platformPrefabs.Length);
            return platformPrefabs[random];
        }
    }
}
