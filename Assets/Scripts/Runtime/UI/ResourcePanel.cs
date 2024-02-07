using Core.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class ResourcePanel : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private TMP_Text _coinsTMP;
        [SerializeField] private TMP_Text _foodTMP;

        [Header("Dev Buttons")]
        [SerializeField] private UIButton _devPlusCoinsButton;
        [SerializeField] private UIButton _devMinusCoinsButton;

        [Space]
        [SerializeField] private UIButton _devPlusFoodButton;
        [SerializeField] private UIButton _devMinusFoodButton;
        
        private PlayerData _playerData;

        #region MonoBehaviour

        private void OnEnable()
        {
            DisplayResources();

#if REVENKO_DEVELOP
            _devPlusCoinsButton.OnClicked += PlusCoins;
            _devMinusCoinsButton.OnClicked += MinusCoins;
            
            _devPlusFoodButton.OnClicked += PlusFood;
            _devMinusFoodButton.OnClicked += MinusFood;
#endif
        }

#if REVENKO_DEVELOP
        private void OnDisable()
        {
            _devPlusCoinsButton.OnClicked -= PlusCoins;
            _devMinusCoinsButton.OnClicked -= MinusCoins;
            
            _devPlusFoodButton.OnClicked -= PlusFood;
            _devMinusFoodButton.OnClicked -= MinusFood;
        }
#endif

        #endregion

        [Inject]
        private void Construct(PlayerData playerData) =>
            _playerData = playerData;

        public void DisplayResources()
        {
            _coinsTMP.SetText(_playerData.CoinsAmount.ToString());
            _foodTMP.SetText(_playerData.FoodAmount.ToString());
        }

#if REVENKO_DEVELOP
        private void PlusCoins()
        {
            _playerData.AddCoin();
            DisplayResources();
        }

        private void MinusCoins()
        {
            _playerData.RemoveCoins(1);
            DisplayResources();
        }

        private void PlusFood()
        {
            _playerData.AddFood();
            DisplayResources();
        }

        private void MinusFood()
        {
            _playerData.RemoveFood(1);
            DisplayResources();
        }
#endif
    }
}