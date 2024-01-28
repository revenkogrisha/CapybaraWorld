using Core.Common.GameNotification;
using Core.Common;
using UnityEngine;
using Zenject;
using Core.Common.ThirdParty;

namespace Core.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [SerializeField] private NotificationCollection _notifications;
        
        [Space]
        [SerializeField] private ParticlesCollection _particles;

        public override void InstallBindings()
        {
            BindThirdPartyInitializer();

#if UNITY_ANDROID && !UNITY_EDITOR
            BindNotifications();
#endif
            BindParticlesHelper();
        }

        private void BindThirdPartyInitializer()
        {
            Container   
                .Bind<ThirdPartyInitializer>()
                .FromNew()
                .AsSingle()
                .Lazy();
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        private void BindNotifications()
        {
            Container   
                .Bind<Notifications>()
                .FromNew()
                .AsSingle()
                .WithArguments(_notifications)
                .Lazy();
        }
#endif

        private void BindParticlesHelper()
        {
            Container   
                .Bind<ParticlesHelper>()
                .FromNew()
                .AsSingle()
                .WithArguments(_particles)
                .Lazy();
        }
    }
}