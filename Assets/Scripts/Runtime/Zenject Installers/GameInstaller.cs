using Core.Game;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindScore();
            BindGameOverHandler();
        }

        private void BindScore()
        {
            Container  
                .Bind<Score>()
                .FromNew()
                .AsSingle()
                .Lazy();
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
