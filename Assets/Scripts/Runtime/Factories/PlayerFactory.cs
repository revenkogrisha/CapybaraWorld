using Core.Player;
using Zenject;

namespace Core.Factories
{
    public class PlayerFactory : IFactory<Hero>
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

        public Hero Create()
        {
            Hero hero = _diContainer
                .InstantiatePrefabForComponent<Hero>(_config.PlayerPrefab);

            hero.transform.position = _config.PlayerSpawnPosition;
            return hero;
        }
    }
}
