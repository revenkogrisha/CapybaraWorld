#if UNITY_ANDROID && !UNITY_EDITOR
using Zenject;

namespace Core.Common.Notifications
{
    public class Notifications
    {
        public NotificationCollection Collection { get; private set; }

        [Inject]
        public Notifications(NotificationCollection collection)
        {
            Collection = collection;

            AndroidNotificationsSender.Initialize().Forget();
        }

        public void Send(NotificationPreset preset)
        {
            AndroidNotificationsSender.SendDefaultChannel(preset.Id,
                preset.Title,
                preset.Text,
                preset.HoursSendDelay,
                preset.HoursRepeatInterval);
        }
    }
}
#endif