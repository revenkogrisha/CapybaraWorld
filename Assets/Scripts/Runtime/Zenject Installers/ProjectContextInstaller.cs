using Core.Game.Input;
using Core.Saving;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private TouchInputCollection _touchInputCollection;

        public override void InstallBindings()
        {
            BindSaveSystem();
            BindSaveService();
            
            BindInputHandler();
        }

        private void BindSaveSystem()
        {
            Container
                .Bind<ISaveSystem>()
                .To<JsonSaveSystem>()
                .FromNew()
                .AsTransient()
                .Lazy();
        }

        private void BindSaveService()
        {
            Container
                .Bind<ISaveService>()
                .To<SaveService>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

        private void BindInputHandler()
        {
            Container
                .Bind<InputHandler>()
                .To<TouchInputHandler>()
                .FromNew()
                .AsSingle()
                .WithArguments(_touchInputCollection)
                .NonLazy();
        }
    }
}