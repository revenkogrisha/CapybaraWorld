using Core.Editor.Debugger;
using Core.Other;
using Core.Player;
using Zenject;

namespace Core.Factories
{
    public class PlayerFactory : IFactory<Hero>
    {
        private readonly PlayerAssets _assets;
        private readonly HeroSkins _heroSkins;
        private readonly DiContainer _diContainer;

        [Inject]
        public PlayerFactory(
            HeroSkins heroSkins,
            DiContainer diContainer,
            PlayerAssets assets)
        {
            _heroSkins = heroSkins;
            _diContainer = diContainer;
            _assets = assets;
        }

        public Hero Create()
        {
            Hero hero = _diContainer
                .InstantiatePrefabForComponent<Hero>(_assets.PlayerPrefab);

            hero.SetPosition(_assets.PlayerSpawnPosition);
            SetSkin(hero);

            return hero;
        }

        private void SetSkin(Hero hero)
        {
            HeroSkinSetter skinSetter = hero.GetComponent<HeroSkinSetter>();
            if (skinSetter != null)
                skinSetter.Set(_heroSkins.Current);
            else
                RDebug.Error($"{nameof(PlayerFactory)}: No Skin Setter is found on Hero! Unable to resolve skin!");
        }
    }
}
