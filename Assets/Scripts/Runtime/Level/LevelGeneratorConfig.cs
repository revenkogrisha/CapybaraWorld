using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "Level Generator Config", menuName = "Configs/Level Generator Config")]
    public class LevelGeneratorConfig : ScriptableObject
    {
        [Header("Generation Settings")]
        [SerializeField] private int _specialPlatformSequentialNumber = 4;

        [Header("Platforms Settings")]
        [SerializeField] private int _platformsStartAmount = 5;
        [SerializeField] private float _xStartPoint = 0f;
        [SerializeField] private float _platformsY = -2f;

        [Header("Platforms")]
        [SerializeField] private SimplePlatform _startPlatform;
        [SerializeField] private Location[] _locations;

        public int SpecialPlatformSequentialNumber => _specialPlatformSequentialNumber;
        public int PlatformsStartAmount => _platformsStartAmount;
        public float XtartPoint => _xStartPoint;
        public float PlatformsY => _platformsY;
        public SimplePlatform StartPlatform => _startPlatform;
        public Location[] Locations => _locations;
    }
}
