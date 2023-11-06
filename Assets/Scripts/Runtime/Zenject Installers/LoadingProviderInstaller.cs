using Core.Common;
using Zenject;

namespace Core.Installers
{
    public class LoadingProviderInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container   
                .Bind<LoadingProvider>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}
