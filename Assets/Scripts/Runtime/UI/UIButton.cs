﻿using System;
using Core.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace UnityTools.Buttons
{
    public class UIButton : TweenClickable
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

        private void InvokeOnClicked() => 
            OnClicked?.Invoke();
    }
}