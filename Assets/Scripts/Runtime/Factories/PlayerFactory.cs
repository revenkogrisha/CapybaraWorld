using Core.Other;
using Core.Player;
using Zenject;

namespace Core.Factories
{
    public class PlayerFactory : IFactory<Hero>
    {
        private readonly PlayerAssets _assets;
        private readonly DiContainer _diContainer;

        [Inject]
        public PlayerFactory(
            PlayerAssets assets,
            DiContainer diContainer)
        {
            _assets = assets;
            _diContainer = diContainer;
        }

        public Hero Create()
        {
            Hero hero = _diContainer
                .InstantiatePrefabForComponent<Hero>(_assets.PlayerPrefab);

            hero.SetPosition(_assets.PlayerSpawnPosition);
            return hero;
        }
    }
}
