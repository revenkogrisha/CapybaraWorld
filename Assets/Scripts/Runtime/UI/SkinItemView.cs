using Core.Editor.Debugger;
using Core.Player;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class SkinItemView : MonoBehaviour, 
        IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        private const float ClickScale = 0.85f;
        private const float ClickTweenDuration = 0.08f;
        
        [SerializeField] private GameObject _selectedPanel;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameTMP;

        private SkinsPanel _panel;
        private bool _isTweeningScale = false;

        public SkinName Name { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isTweeningScale == false)
            {
                _isTweeningScale = true;
                transform.DOScale(ClickScale, ClickTweenDuration);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_isTweeningScale == true)
            {
                transform.DOScale(1f, ClickTweenDuration);
                _isTweeningScale = false;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_panel == null)
            {
                RDebug.Warning($"{nameof(SkinItemView)}::{nameof(OnPointerClick)}: root panel is null when clicking on item view!");
                return;
            }

            _panel.InvokeItemDisplay(Name);
        }

        public void Initialize(SkinsPanel panel, SkinName skinName, Sprite menuItem)
        {
            _panel = panel;

            Name = skinName;

            _image.sprite = menuItem;
            _nameTMP.SetText(Name.ToString());

            OnPointerUp(null);
        }

        public void SetSelected(bool value) => 
            _selectedPanel.SetActive(value);
    }
}