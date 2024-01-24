using TriInspector;
using UnityEngine;

namespace Core.Common.Notifications
{
    [CreateAssetMenu(fileName = "Notification", menuName = "Presets/Notification")]
    public class NotificationPreset : ScriptableObject
    {
        private const int MinId = 0;
        private const int MaxId = 100000;
        
        [InfoBox("Use '" + nameof(GenerateID) + "' button to generate an UNIQUE Id")]
        [Title("Id"), HideLabel, ReadOnly]
        [SerializeField] private int _id;
        
        [Title("General")]
        [SerializeField] private string _title;
        [SerializeField] private string _text;

        [Title("Timing")]
        [SerializeField] private double _hoursSendDelay = 8d;
        [SerializeField, Min(-1f)] private double _hoursRepeatInterval = -1d;

        public int Id => _id;
        public string Title => _title;
        public string Text => _text;
        public double HoursSendDelay => _hoursSendDelay;
        public double HoursRepeatInterval => _hoursRepeatInterval;

        [PropertySpace(15f), Button(ButtonSizes.Medium)]
        private void GenerateID() =>
            _id = Random.Range(MinId, MaxId);
    }
}