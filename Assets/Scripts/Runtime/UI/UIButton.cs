using System;
using Core.Common;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class UIButton : TweenClickable
    {
        private const float LockShakeDuration = 0.3f;
        private const float LockShakeStrength = 9f;
        private const int LockShakeVibration = 10;
    
        [Header("Components")]
        [SerializeField] private Button _button;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _tmp;

        [Header("Graphics")]
        [SerializeField] private Sprite _normalSprite;
        [SerializeField] private Sprite _disabledSprite;

        [Space]
        [SerializeField] private Color _textNormalColor = Color.white;
        [SerializeField] private Color _textDisabledColor = Color.grey;

        public bool Interactable
        {
            get => _button.interactable;
            set
            {
                _button.interactable = value;
                _image.sprite = value == true
                    ? _normalSprite
                    : _disabledSprite;

                _tmp.color = value == true
                    ? _textNormalColor
                    : _textDisabledColor;
            }
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
            else
                transform.DOShakePosition(LockShakeDuration, LockShakeStrength, LockShakeVibration);
        }

        private void PerformClick()
        {
            HapticHelper.VibrateLight();
            OnClicked?.Invoke();
        }
    }
}