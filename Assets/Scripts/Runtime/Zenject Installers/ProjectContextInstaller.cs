using Core.Game.Input;
using Core.Mediation;
using Core.Saving;
using UnityEngine;
using YG;
using Zenject;

namespace Core.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        [SerializeField] private KeyInputCollection _keyInputCollection;

        private void OnEnable() => 
            YandexGame.GetDataEvent += BindInputHandler;

        private void OnDisable() => 
            YandexGame.GetDataEvent -= BindInputHandler;

        public override void InstallBindings()
        {
            BindSaveSystem();
            BindCloudSaveSystem();

            BindSaveService();
            
            if (YandexGame.SDKEnabled == true)
                BindInputHandler();

            BindMediationService();
        }

        private void BindSaveSystem()
        {
            Container
                .Bind<ISaveSystem>()
                .To<YGSaveSystem>()
                .FromNew()
                .AsTransient()
                .Lazy();
        }

        private void BindCloudSaveSystem()
        {
            Container
                .Bind<ICloudSaveSystem>()
                .To<FakeCloudSaveSystem>()
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
            if (YandexGame.EnvironmentData.deviceType.Equals("mobile"))
            {
                Container
                    .Bind<InputHandler>()
                    .To<ButtonsInputHandler>()
                    .FromNew()
                    .AsSingle()
                    .NonLazy();
            }
            else
            {
                Container
                    .Bind<InputHandler>()
                    .To<KeyboardInputHandler>()
                    .FromNew()
                    .AsSingle()
                    .WithArguments(_keyInputCollection)
                    .NonLazy();
            }
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