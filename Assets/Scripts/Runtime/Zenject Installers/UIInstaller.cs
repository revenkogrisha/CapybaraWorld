using Core.Factories;
using Core.UI;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIRoot _uiRoot;
        [SerializeField] private UICollection _uiCollection;

        public override void InstallBindings()
        {
            BindUIRoot();
            BindUIProvider();

            BindMainMenu();
        }

        private void BindUIRoot()
        {
            Container
                .Bind<IUIRoot>()
                .To<UIRoot>()
                .FromInstance(_uiRoot)
                .AsSingle()
                .Lazy();
        }

        private void BindUIProvider()
        {
            Container
                .Bind<UIProvider>()
                .FromNew()
                .AsSingle()
                .WithArguments(_uiCollection)
                .Lazy();
        }

        private void BindMainMenu()
        {
            Container
                .Bind<MainMenu>()
                .FromNew()
                .AsSingle()
                .Lazy();

#if REVENKO_DEVELOP
            Container
                .Bind<MainMenuDevHandler>()
                .FromNew()
                .AsSingle()
                .Lazy();
#endif
        }
    }
}