using Core.Game;
using Zenject;

namespace Core.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGameOverHandler();
        }

        private void BindGameOverHandler()
        {
            Container
                .Bind<GameOverHandler>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}
