using UnityEngine;

namespace Core.Common
{
    public class DevObjectsHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _objects;

        private bool _isDevEnvironment;

        private void Awake()
        {
#if REVENKO_DEVELOP
            _isDevEnvironment = true;
#else
            _isDevEnvironment = false;
#endif
            Setup();
        }

        private void Setup()
        {
            foreach (GameObject item in _objects) 
                item.SetActive(_isDevEnvironment);
        }
    }
}