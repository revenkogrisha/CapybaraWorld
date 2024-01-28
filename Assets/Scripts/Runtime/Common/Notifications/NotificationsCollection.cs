using UnityEngine;

namespace Core.Common.GameNotification
{
    [CreateAssetMenu(fileName = "Notifications Collection", menuName = "Collections/Notifications")]
    public class NotificationCollection : ScriptableObject
    {
        [SerializeField] private NotificationPreset _test;
        [SerializeField] private NotificationPreset _delayedTest;

        [Space]
        [SerializeField] private NotificationPreset _playReminder;
        [SerializeField] private NotificationPreset _locations;

        public NotificationPreset Test => _test;
        public NotificationPreset DelayedTest => _delayedTest;

        public NotificationPreset PlayReminder => _playReminder;
        public NotificationPreset Locations => _locations;
    }
}
