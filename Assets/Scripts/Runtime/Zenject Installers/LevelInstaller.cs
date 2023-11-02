using Core.Level;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelGeneratorConfig _levelGeneratorConfig;
        [SerializeField] private Transform _platformsParent;

        public override void InstallBindings()
        {
            BindLevelGenerator();
        }

        private void BindLevelGenerator()
        {
            Container
                .Bind<ILevelGenerator>()
                .To<LevelGenerator>()
                .FromNew()
                .AsSingle()
                .WithArguments(_levelGeneratorConfig, _platformsParent)
                .Lazy();
        }
    }
}
