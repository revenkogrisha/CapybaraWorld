using Core.Common.GameNotification;
using Core.Common;
using UnityEngine;
using Zenject;
using Core.Common.ThirdParty;
using Core.Audio;
using TriInspector;

namespace Core.Installers
{
    public class CommonInstaller : MonoInstaller
    {
        [Title("Notifications Collection"), HideLabel]
        [SerializeField] private NotificationCollection _notifications;
        
        [Title("Particles Collection"), HideLabel]
        [SerializeField] private ParticlesCollection _particles;

        [Title("Audio Handler"), HideLabel]
        [SerializeField, SceneObjectsOnly] private UnityAudioHandler _audioHandler;

        public override void InstallBindings()
        {
            BindThirdPartyInitializer();

#if UNITY_ANDROID && !UNITY_EDITOR
            BindNotifications();
#endif
            BindParticlesHelper();
            BindAudioHandler();
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

        private void BindAudioHandler()
        {
            Container   
                .Bind<IAudioHandler>()
                .To<UnityAudioHandler>()
                .FromInstance(_audioHandler)
                .AsSingle()
                .NonLazy();
        }
    }
}