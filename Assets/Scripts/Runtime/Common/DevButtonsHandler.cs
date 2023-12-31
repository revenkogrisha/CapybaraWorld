using UnityEngine;
using UnityTools.Buttons;

namespace Core.Common
{
    public class DevButtonsHandler : MonoBehaviour
    {
        [SerializeField] private UIButton[] _buttons;

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
            foreach (UIButton button in _buttons) 
                button.gameObject.SetActive(_isDevEnvironment);
        }
    }
}