using Core.Infrastructure;
using Zenject;

namespace Core.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGlobalStateMachine();
        }

        private void BindGlobalStateMachine()
        {
            Container
                .Bind<IGlobalStateMachine>()
                .To<GlobalStateMachine>()
                .FromNew()
                .AsSingle()
                .NonLazy();
        }
    }
}
