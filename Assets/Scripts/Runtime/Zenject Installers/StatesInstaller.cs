using Core.Infrastructure;
using Zenject;

namespace Core.Installers
{
    public class StatesInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindGenerationState();
            BindMainMenuState();
            BindGameplayState();
            BindGameOverState();
        }

        private void BindGenerationState()
        {
            Container
                .Bind<GenerationState>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindMainMenuState()
        {
            Container
                .Bind<MainMenuState>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindGameplayState()
        {
            Container
                .Bind<GameplayState>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindGameOverState()
        {
            Container
                .Bind<GameOverState>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}
