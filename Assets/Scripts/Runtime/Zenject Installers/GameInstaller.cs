using Core.Game;
using Core.Game.Input;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private TouchInputCollection _touchInputCollection;

        public override void InstallBindings()
        {
            BindInputHandler();
            
            BindScore();
            BindPlaythroughHandler();
        }

        private void BindInputHandler()
        {
            Container
                .Bind<InputHandler>()
                .To<ButtonsInputHandler>()
                .FromNew()
                .AsSingle()
                .NonLazy();

            // Bind Touch input handler
            // Container
            //     .Bind<InputHandler>()
            //     .To<TouchInputHandler>()
            //     .FromNew()
            //     .AsSingle()
            //     .WithArguments(_touchInputCollection)
            //     .NonLazy();
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