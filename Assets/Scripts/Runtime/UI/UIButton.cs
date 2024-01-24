using System;
using Core.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIButton : TweenClickable
    {
        [Header("Components")]
        [SerializeField] private Button _button;

        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }

        public event Action OnClicked;

        #region MonoBehaviour

        private void OnEnable() => 
            _button.onClick.AddListener(PerformClick);

        private void OnDisable() => 
            _button.onClick.RemoveAllListeners();

        #endregion

        public override void OnPointerDown(PointerEventData eventData)
        {
            if (Interactable == true)
                base.OnPointerDown(eventData);
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            if (Interactable == true)
                base.OnPointerUp(eventData);
        }

        private void PerformClick()
        {
            OnClicked?.Invoke();
            HapticHelper.VibrateLight();
        }
    }
}