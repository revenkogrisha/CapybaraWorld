using Core.Common;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField] private ParticlesCollection _particles;

        public override void InstallBindings()
        {
            BindParticlesHelper();
        }

        private void BindParticlesHelper()
        {
            Container   
                .Bind<ParticlesHelper>()
                .FromNew()
                .AsSingle()
                .WithArguments(_particles)
                .Lazy();
        }
    }
}