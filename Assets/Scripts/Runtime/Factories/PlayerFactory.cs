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
            SetAccentColor(hero);

            return hero;
        }

        private void SetAccentColor(Hero hero)
        {
            if (hero.TryGetComponent(out AccentColorHandler colorHandler) == true)
                colorHandler.AccentColor = _heroSkins.Current.AccentColor;
            else
                RDebug.Error($"{nameof(PlayerFactory)}: No {nameof(AccentColorHandler)} is found on Hero! Unable to resolve accent color from Skin Preset!");
        }

        private void SetSkin(Hero hero)
        {
            if (hero.TryGetComponent(out HeroSkinSetter skinSetter) == true)
                skinSetter.Set(_heroSkins.Current);
            else
                RDebug.Error($"{nameof(PlayerFactory)}: No Skin Setter is found on Hero! Unable to resolve skin!");
        }
    }
}
