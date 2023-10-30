using Core.Level;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private LevelGeneratorConfig _levelGeneratorConfig;

        public override void InstallBindings()
        {
            BindLevelGenerator();
        }

        private void BindLevelGenerator()
        {
            LevelGenerator instance = new(_levelGeneratorConfig);
            
            Container
                .Bind<ILevelGenerator>()
                .To<LevelGenerator>()
                .FromInstance(instance)
                .AsSingle()
                .Lazy();
        }
    }
}
