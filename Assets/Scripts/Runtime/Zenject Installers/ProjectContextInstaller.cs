using Core.Mediation;
using Core.Mediation.UnityAds;
using Core.Saving;
using Zenject;

namespace Core.Installers
{
    public class ProjectContextInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindSaveSystem();
            BindCloudSaveSystem();

            BindSaveService();

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

        private void BindMediationService()
        {
            Container
                .Bind<IMediationService>()
                .To<UnityAdsService>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }
    }
}