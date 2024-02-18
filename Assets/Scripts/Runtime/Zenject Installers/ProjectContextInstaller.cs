using Core.Game.Input;
using Core.Mediation;
using Core.Saving;
using UnityEngine;
using Zenject;

namespace Core.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private KeyInputCollection _keyInputCollection;

        public override void InstallBindings()
        {
            BindSaveSystem();
            BindCloudSaveSystem();

            BindSaveService();
            
            BindInputHandler();

            BindMediationService();
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

        private void BindCloudSaveSystem()
        {
            Container
                .Bind<ICloudSaveSystem>()
#if UNITY_ANDROID && !UNITY_EDITOR
                .To<GooglePlayGamesSaveSystem>()
#else
                .To<FakeCloudSaveSystem>()
#endif
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
                .To<KeyboardInputHandler>()
                .FromNew()
                .AsSingle()
                .WithArguments(_keyInputCollection)
                .NonLazy();
        }

        private void BindMediationService()
        {
            Container
                .Bind<IMediationService>()
                .To<YGService>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}