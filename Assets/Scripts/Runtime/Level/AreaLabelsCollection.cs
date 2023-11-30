using TriInspector;
using UnityEngine;

namespace Core.Level
{
    [CreateAssetMenu(fileName = "Area Labels", menuName = "Collections/Area Labels")]
    public class AreaLabelsCollection : ScriptableObject
    {
        [ListDrawerSettings(AlwaysExpanded = true)] 
        [SerializeField] private string[] _firstParts;
        
        [ListDrawerSettings(AlwaysExpanded = true)] 
        [SerializeField] private string[] _lastParts;

        public string[] FirstParts => _firstParts;
        public string[] LastParts => _lastParts;
    }
}