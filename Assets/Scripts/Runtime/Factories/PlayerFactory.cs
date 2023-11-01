using Core.Player;
using UnityEngine;
using Zenject;

namespace Core.Factories
{
    public class PlayerFactory : IFactory<PlayerTest>
    {
        private readonly PlayerConfig _config;
        private readonly DiContainer _diContainer;

        [Inject]
        public PlayerFactory(
            PlayerConfig playerConfig,
            DiContainer diContainer)
        {
            _config = playerConfig;
            _diContainer = diContainer;
        }

        public PlayerTest Create()
        {
            MiddleObject middleObject = Object.Instantiate(_config.MiddleObjectPrefab);

            PlayerTest hero = _diContainer
                .InstantiatePrefabForComponent<PlayerTest>(_config.PlayerPrefab);

            hero.transform.position = _config.PlayerSpawnPosition;
            hero.Initialize(middleObject);

            return hero;
        }
    }
}
