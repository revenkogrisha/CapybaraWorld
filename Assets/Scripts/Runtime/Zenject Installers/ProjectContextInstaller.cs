using Core.Game.Input;
using Core.Infrastructure;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private TouchInputCollection _touchInputCollection;

        public override void InstallBindings()
        {
            BindGlobalStateMachine();
            BindInputHandler();
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
