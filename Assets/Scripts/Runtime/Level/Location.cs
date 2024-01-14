using TriInspector;
using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "New Location", menuName = "Presets/Location")]
    public class Location : ScriptableObject
    {
        [Required(FixAction = nameof(FixName), FixActionName = "Assign object name"), 
        ValidateInput(nameof(ValidateName))]
        [SerializeField] private string _name;

        [SerializeField, Required] private BackgroundPreset _backgroundPreset;

        [Space]
        [SerializeField, Required] private Platform _startPlatform;

        [Space]
        [InfoBox("Empty - add at least one!", TriMessageType.Error, nameof(IsEnoughSimplePlatforms))]
        [SerializeField] private SimplePlatform[] _simplePlatforms;
        [InfoBox("Empty - add few platforms!", TriMessageType.Error, nameof(IsEmptySpecialPlatforms))]
        [InfoBox("Very few platforms", TriMessageType.Warning, nameof(IsEnoughSpecialPlatforms))]
        [SerializeField] private SpecialPlatform[] _specialPlatforms;

        public string Name => _name;
        public BackgroundPreset BackgroundPreset => _backgroundPreset;
        
        public Platform StartPlatform => _startPlatform;
        
        public SimplePlatform[] SimplePlatforms => _simplePlatforms;
        public SpecialPlatform[] SpecialPlatforms => _specialPlatforms;

        private bool IsEnoughSimplePlatforms => _simplePlatforms.Length <= 0;
        private bool IsEmptySpecialPlatforms => _specialPlatforms.Length <= 0;
        private bool IsEnoughSpecialPlatforms => _specialPlatforms.Length == 1;

        private void FixName() =>
            _name = name;
        
        private TriValidationResult ValidateName()
        {
            if (_name.Equals(name) == false && string.IsNullOrEmpty(_name) == false)
                return TriValidationResult.Warning("Possible name mismatch");
            else
                return TriValidationResult.Valid;
        }
    }
}