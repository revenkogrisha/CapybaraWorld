using UnityEngine;

namespace Core.Level
{
    [SelectionBase]
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected bool Preloaded = false;
        [SerializeField] private bool _poolable = true;

        public bool Poolable => _poolable;
    }
}
