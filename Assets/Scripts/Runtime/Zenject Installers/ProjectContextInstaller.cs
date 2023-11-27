using Core.Game.Input;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private TouchInputCollection _touchInputCollection;

        public override void InstallBindings() => 
            BindInputHandler();

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