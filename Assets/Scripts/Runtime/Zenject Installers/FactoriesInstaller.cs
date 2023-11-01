using Core.Factories;
using Core.Player;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        [SerializeField] private PlayerConfig _playerConfig;

        public override void InstallBindings()
        {
            BindPlayerFactory();
            BindPlayerDeadlineFactory();
        }

        private void BindPlayerFactory()
        {
            Container
                .Bind<PlayerFactory>()
                .FromNew()
                .AsSingle()
                .WithArguments(_playerConfig)
                .Lazy();
        }

        private void BindPlayerDeadlineFactory()
        {
            Container
                .Bind<PlayerDeadlineFactory>()
                .FromNew()
                .AsSingle()
                .WithArguments(_playerConfig)
                .Lazy();
        }
    }
}