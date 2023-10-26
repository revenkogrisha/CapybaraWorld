using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "Level Generator Config", menuName = "Configs/Level Generator Config")]
    public class LevelGeneratorConfig : ScriptableObject
    {
        [Header("Platform Generation Settings")]
        [SerializeField] private Transform _platformsParent;
        [SerializeField] private int _platformsAmountToGenerate = 5;
        [SerializeField] private float _xStartPoint = 0f;
        [SerializeField] private float _platformsY = -2f;

        [Header("Platforms")]
        [SerializeField] private SimplePlatform _startPlatform;
        [SerializeField] private SpecialPlatform _questPlatform;
        [SerializeField] private Location[] _locations;

        public Transform PlatformsParent => _platformsParent;
        public int PlatformsAmountToGenerate => _platformsAmountToGenerate;
        public float XtartPoint => _xStartPoint;
        public float PlatformsY => _platformsY;
        public SimplePlatform StartPlatform => _startPlatform;
        public Location[] Locations => _locations;
    }
}
