using Core.Level;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class LevelInstaller : MonoInstaller
    {
        [SerializeField] private EntityAssets _entityAssets;
        [SerializeField] private EnemyAssets _enemyAssets;

        [Space]
        [SerializeField] private LevelGeneratorConfig _levelGeneratorConfig;
        [SerializeField] private AreaLabelsCollection _areaLabels;

        [Space]
        [SerializeField] private Transform _platformsParent;

        public override void InstallBindings()
        {
            BindEntitiesSpawner();
            BindLevelGenerator();
        }

        private void BindEntitiesSpawner()
        {
            Container
                .Bind<EntitySpawner>()
                .FromNew()
                .AsSingle()
                .WithArguments(_entityAssets, _enemyAssets)
                .Lazy();
        }

        private void BindLevelGenerator()
        {
            Container
                .Bind(typeof(ILevelGenerator), typeof(ILocationsHandler))
                .To<LevelGenerator>()
                .FromNew()
                .AsSingle()
                .WithArguments(_levelGeneratorConfig, _areaLabels, _platformsParent)
                .Lazy();
        }
    }
}
