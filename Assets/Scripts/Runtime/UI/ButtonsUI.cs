using UnityEngine;

namespace Core.UI
{
    public class ButtonsUI : MonoBehaviour
    {
        [SerializeField] private UIButton _left;
        [SerializeField] private UIButton _right;
        [SerializeField] private UIButton _jump;
        [SerializeField] private UIButton _dash;
        [SerializeField] private UIButton _descend;

        public UIButton Left => _left;
        public UIButton Right => _right;
        public UIButton Jump => _jump;
        public UIButton Dash => _dash;
        public UIButton Descend => _descend;
    }
}