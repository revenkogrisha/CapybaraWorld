using TriInspector;
using UnityEngine;

namespace Core.Player
{
    [CreateAssetMenu(fileName = "Skins Collection", menuName = "Collections/Skins")]
    public class SkinsCollection : ScriptableObject
    {
        [SerializeField, ListDrawerSettings(AlwaysExpanded = true)] private SkinPreset[] _presets;

        public SkinPreset[] Presets => _presets;
    }
}