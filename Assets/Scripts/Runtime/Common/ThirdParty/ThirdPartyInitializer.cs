#if UNITY_ANDROID && !UNITY_EDITOR
#define ANDROID_RUNTIME
#else
#undef ANDROID_RUNTIME
#endif

#if ANDROID_RUNTIME
using Core.Common.GameNotification;
using System.Threading;
using Core.Common;
using Core.Other;
using Google.Play.AppUpdate;
#endif
using Zenject;
using Core.Editor.Debugger;
using Core.Mediation;
using Cysharp.Threading.Tasks;

namespace Core.Common.ThirdParty
{

    public class ThirdPartyInitializer
    {
        private readonly IMediationService _mediationService;
#if ANDROID_RUNTIME
        private readonly Notifications _notifications;
#endif

        [Inject]
        public ThirdPartyInitializer(
#if ANDROID_RUNTIME
            Notifications notifications,
#endif
            IMediationService mediationService)
        {
#if ANDROID_RUNTIME
            _notifications = notifications;
#endif
            _mediationService = mediationService;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async UniTaskVoid InitializeAll()
#pragma warning restore CS1998 
        {
            _mediationService.Initialize();

            FirebaseService.Initialize();
            
#if ANDROID_RUNTIME
            await SignInService.Authenticate();

            ScheduleAndroidNotifications();
            await HandleAppUpdate();
#endif

            RDebug.Info($"{nameof(ThirdPartyInitializer)}: Initialization complete!");
        }

#if ANDROID_RUNTIME
        private async UniTask HandleAppUpdate()
        {
            const float requestTimeout = 60f;
            CancellationTokenSource cts = new();

            IAppUpdateService updateService = new AppUpdateService();
            updateService.Initialize();
            
            cts.CancelByTimeout(requestTimeout).Forget();
            UpdateAvailability result = await updateService.StartUpdateCheck(cts.Token);

            if (result == UpdateAvailability.UpdateAvailable)
                await updateService.Request(cts.Token);
        }

        // Unity Notifications are not exactly "third party", but put it here for convenience
        private void ScheduleAndroidNotifications()
        {
            _notifications.Send(_notifications.Collection.PlayReminder);
            _notifications.Send(_notifications.Collection.Locations);
        }
#endif
    }
}