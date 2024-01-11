using Core.Editor.Debugger;
using Core.Player;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Core.UI
{
    public class SkinItemView : MonoBehaviour, IPointerClickHandler
    {
        private SkinsPanel _panel;
        
        [Space]
        [SerializeField] private GameObject _selectedPanel;
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _nameTMP;

        public SkinName Name { get; private set; }

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
        }

        public void SetSelected(bool value) => 
            _selectedPanel.SetActive(value);
    }
}