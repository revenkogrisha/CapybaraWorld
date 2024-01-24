using UnityEngine;

namespace Core.Common.Notifications
{
    [CreateAssetMenu(fileName = "Notifications Collection", menuName = "Collections/Notifications")]
    public class NotificationCollection : ScriptableObject
    {
        [SerializeField] private NotificationPreset _test;

        [Space]
        [SerializeField] private NotificationPreset _playReminder;
        [SerializeField] private NotificationPreset _locations;

        public NotificationPreset Test => _test;
        public NotificationPreset PlayReminder => _playReminder;
        public NotificationPreset Locations => _locations;
    }
}
