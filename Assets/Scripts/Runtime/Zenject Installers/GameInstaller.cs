using Core.Game;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScore();
            BindPlaythroughHandler();
        }

        private void BindScore()
        {
            Container  
                .Bind<Score>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindPlaythroughHandler()
        {
            Container
                .BindInterfacesAndSelfTo<PlaythroughHandler>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}
