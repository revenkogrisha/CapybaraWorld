using TMPro;
using UniRx;
using UnityEngine;
using UnityTools.Buttons;

namespace Core.UI
{
    public class SkinPlacementPanel : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private UIButton _buyButton;
        [SerializeField] private UIButton _selectButton;

        [Header("Cost")]
        [SerializeField] private TMP_Text _costTMP;

        [Header("Colors")]
        [SerializeField] private Color _costAvailableColor = Color.white;
        [SerializeField] private Color _costLockedColor = Color.red;

        public readonly ReactiveCommand BuyButtonCommand = new();
        public readonly ReactiveCommand SelectButtonCommand = new();

        #region MonoBehaviour

        private void OnEnable()
        {
            _buyButton.OnClicked += InvokeBuyButtonEvent;
            _selectButton.OnClicked += InvokeSelectButtonEvent;
        }

        private void OnDisable()
        {
            _buyButton.OnClicked -= InvokeBuyButtonEvent;
            _selectButton.OnClicked -= InvokeSelectButtonEvent;
        }

        #endregion

        public void SetBuyState(int cost, bool canBuy)
        {
            _selectButton.gameObject.SetActive(false);
            _buyButton.gameObject.SetActive(true);

            _buyButton.Interactable = canBuy;

            _costTMP.text = cost.ToString();
        }

        public void SetSelectableState()
        {
            _buyButton.gameObject.SetActive(false);
            _selectButton.gameObject.SetActive(true);
        }

        public void SetSelectedState()
        {
            _buyButton.gameObject.SetActive(false);
            _selectButton.gameObject.SetActive(false);
        }

        public void SetAvailability(bool state)
        {
            _buyButton.Interactable = state;
            if (state == true)
                _costTMP.color = _costAvailableColor;
            else
                _costTMP.color = _costLockedColor;
        }

        private void InvokeBuyButtonEvent() =>
            BuyButtonCommand.Execute();
        
        private void InvokeSelectButtonEvent() =>
            SelectButtonCommand.Execute();
    }
}