using System;
using UnityEngine;
using UnityEngine.UI;


namespace UnityTools.Buttons
{
    public class UIButton : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private Button _button;

        public bool IsLocked { get; private set; }

        public Button OriginalButton => _button;
        public bool Interactable
        {
            get => _button.interactable;
            set
            {
                if (IsLocked)
                    return;

                _button.interactable = value;
            }
        }

        public event Action OnClicked;

        #region MonoBehaviour

        private void OnEnable() => 
            _button.onClick.AddListener(InvokeOnClicked);

        private void OnDisable() => 
            _button.onClick.RemoveAllListeners();

        #endregion

        private void InvokeOnClicked() => 
            OnClicked?.Invoke();
    }
}