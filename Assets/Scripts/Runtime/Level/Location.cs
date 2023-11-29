using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "New Location", menuName = "Presets/Location")]
    public class Location : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private BackgroundPreset _backgroundPreset;

        [Space]
        [SerializeField] private SimplePlatform[] _simplePlatforms;
        [SerializeField] private SpecialPlatform[] _specialPlatforms;

        public string Name => _name;
        public BackgroundPreset BackgroundPreset => _backgroundPreset;
        public SimplePlatform[] SimplePlatforms => _simplePlatforms;
        public SpecialPlatform[] SpecialPlatforms => _specialPlatforms;
    }
}