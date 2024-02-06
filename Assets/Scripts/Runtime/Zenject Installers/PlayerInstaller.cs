using Core.Player;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private FocusCamera _focusCamera;
        
        [Space]
        [SerializeField] private PlayerUpgradeConfig _upgradeConfig;

        [Space]
        [SerializeField] private SkinsCollection _skins;

        public override void InstallBindings()
        {
            BindPlayerData();
            BindCamera();
            BindUpgrade();
            BindHeroSkins();
        }

        private void BindPlayerData()
        {
            Container
                .Bind<PlayerData>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindCamera()
        {
            Container
                .Bind<IPlayerCamera>()
                .To<FocusCamera>()
                .FromInstance(_focusCamera)
                .AsSingle()
                .Lazy();
        }
        
        private void BindUpgrade()
        {
            Container
                .Bind<PlayerUpgrade>()
                .FromNew()
                .AsSingle()
                .WithArguments(_upgradeConfig)
                .Lazy();
        }
        
        private void BindHeroSkins()
        {
            Container
                .Bind<HeroSkins>()
                .FromNew()
                .AsSingle()
                .WithArguments(_skins)
                .Lazy();
        }
    }
}