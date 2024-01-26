#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using System.Threading;
using Core.Editor.Debugger;
using Core.Other;
using Cysharp.Threading.Tasks;
using Unity.Notifications.Android;
using static Core.Common.Notifications.NotificationsData;

namespace Core.Common.Notifications
{
    public static class AndroidNotificationsSender
    {
        private static PermissionStatus _permissionStatus = PermissionStatus.NotRequested;

        private static bool CanNotify => _permissionStatus == PermissionStatus.Allowed;
        
        public static async UniTaskVoid Initialize()
        {
            try
            {
                const float requestTimeout = 120f;

                CancellationTokenSource cts = new();
                cts.CancelByTimeout(requestTimeout).Forget();

                _permissionStatus = await RequestPermission(cts.Token);

                if (CanNotify == false)
                    return;

                AndroidNotificationCenter.CancelAllDisplayedNotifications();
                
                AndroidNotificationCenter.RegisterNotificationChannelGroup(new()
                {
                    Id = MainGroupId,
                    Name = MainGroupName,
                });
                
                AndroidNotificationCenter.RegisterNotificationChannel(new()
                {
                    Id = DefaultChannelId,
                    Name = DefaultChannelName,
                    Importance = Importance.Default,
                    Description = "Generic notifications",
                    Group = MainGroupId,
                });
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(AndroidNotificationsSender)}::{nameof(Initialize)}: {ex.Message} \n {ex.StackTrace}");
            }
        }

        public static void SendDefaultChannel(int id, string title, string text,
            double hoursSendDelay, double hoursRepeatInterval = -1)
        {
            if (CanNotify == true)
                Send(id, title, text, DefaultChannelId, hoursSendDelay, hoursRepeatInterval);
        }

        private static void Send(int id, string title, string text, string channelId,
            double hoursSendDelay, double hoursRepeatInterval)
        {
            try 
            {
                if (AndroidNotificationCenter.CheckScheduledNotificationStatus(id)
                    == NotificationStatus.Scheduled)
                {
                    return;
                }
                
                AndroidNotificationCenter.SendNotificationWithExplicitID(new()
                {
                    Title = title,
                    Text = text,
                    FireTime = DateTime.Now.AddHours(hoursSendDelay),
                    RepeatInterval = hoursRepeatInterval >= 0
                        ? TimeSpan.FromHours(hoursRepeatInterval)
                        : default
                },
                channelId, id);
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(AndroidNotificationsSender)}::{nameof(Send)}: {ex.Message} \n {ex.StackTrace}");
            }
        }

        private static async UniTask<PermissionStatus> RequestPermission(CancellationToken token)
        {
            PermissionRequest request = new();

            await UniTask.WaitWhile(() => request.Status == PermissionStatus.RequestPending, 
                cancellationToken: token);
            
            return request.Status;
        }
    }
}
#endif