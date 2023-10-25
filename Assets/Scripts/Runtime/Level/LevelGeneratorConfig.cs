using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "Level Generator Config", menuName = "Configs/Level Generator Config")]
    public class LevelGeneratorConfig : ScriptableObject
    {
        [field: Header("Platform Generation Settings")]
        [field: SerializeField] public int PlatformsAmountToGenerate { get; private set; } = 5;
        [field: SerializeField] public float XStartPoint { get; private set; } = 0f;
        [field: SerializeField] public float PlatformsY { get; private set; } = -2f;

        [field: Header("Platforms")]
        [field: SerializeField] public Platform StartPlatform { get; private set; }
        [field: SerializeField] public Platform[] Platforms { get; private set; }
    }
}
