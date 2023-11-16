using Core.Factories;
using Core.Player;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class FactoriesInstaller : MonoInstaller
    {
        [SerializeField] private PlayerAssets _playerAssets;

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
                .WithArguments(_playerAssets)
                .Lazy();
        }

        private void BindPlayerDeadlineFactory()
        {
            Container
                .Bind<PlayerDeadlineFactory>()
                .FromNew()
                .AsSingle()
                .WithArguments(_playerAssets)
                .Lazy();
        }
    }
}