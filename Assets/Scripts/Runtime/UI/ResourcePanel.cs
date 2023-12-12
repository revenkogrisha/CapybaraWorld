using Core.Player;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class ResourcePanel : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsTMP;
        [SerializeField] private TMP_Text _foodTMP;
        
        private PlayerData _playerData;

        private void Start() => 
            DisplayResources();

        [Inject]
        private void Construct(PlayerData playerData) =>
            _playerData = playerData;

        public void DisplayResources()
        {
            _coinsTMP.SetText(_playerData.CoinsAmount.ToString());
            _foodTMP.SetText(_playerData.FoodAmount.ToString());
        }
    }
}