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

        [Header("Background")]
        [SerializeField] private Background _backgroundPrefab;
        [SerializeField] private Vector2 _backgroundPosition = new(0f, 2.5f);

        public int SpecialPlatformSequentialNumber => _specialPlatformSequentialNumber;
        public int PlatformsStartAmount => _platformsStartAmount;
        public float XtartPoint => _xStartPoint;
        public float PlatformsY => _platformsY;
        public SimplePlatform StartPlatform => _startPlatform;
        public Location[] Locations => _locations;
        public Background BackgroundPrefab => _backgroundPrefab;
        public Vector2 BackgroundPosition => _backgroundPosition;
    }
}