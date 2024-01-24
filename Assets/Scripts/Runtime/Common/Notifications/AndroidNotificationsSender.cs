#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using Core.Editor.Debugger;
using Unity.Notifications.Android;
using static Core.Common.Notifications.NotificationsData;

namespace Core.Common.Notifications
{
    public static class AndroidNotificationsSender
    {
        public static void Initialize()
        {
            try
            {
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
    }
}
#endif