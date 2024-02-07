using Core.Infrastructure;
using Zenject;

namespace Core.Installers
{
    public class GameStateMachineInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindStateMachine();
            BindStates();
            BindStatesProvider();

            BindNavigation();
        }

        private void BindStateMachine()
        {
            Container
                .Bind<IGameStateMachine>()
                .To<GameStateMachine>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindStates()
        {
            BindDataInitializationState();
            BindGenerationState();

            BindMainMenuState();
            BindGameplayState();
            
            BindGameWinState();
            BindGameLostState();
        }

        private void BindStatesProvider()
        {
            Container
                .Bind<GameStatesProvider>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindNavigation()
        {
            Container
                .Bind<GameNavigation>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindDataInitializationState()
        {
            Container
                .Bind<DataInitializationState>()
                .FromNew()
                .AsSingle()
                .Lazy();
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

        private void BindGameWinState()
        {
            Container
                .Bind<GameWinState>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindGameLostState()
        {
            Container
                .Bind<GameLostState>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}
