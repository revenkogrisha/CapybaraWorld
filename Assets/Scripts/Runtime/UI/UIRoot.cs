using UnityEngine;

namespace Core.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class UIRoot : MonoBehaviour, IUIRoot
    {
        private RectTransform _rectTransform;

        RectTransform IUIRoot.RectTransform => _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }
}
