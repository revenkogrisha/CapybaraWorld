using Core.Player;
using Core.Saving;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityTools.Buttons;
using Zenject;

namespace Core.UI
{
    public class UpgradeMenu : AnimatedUI
    {
        private const string CostFormat = "{0}";
        private const string HeroLevelFormat = "Hero Level: {0}";
        
        [Header("Upgrade UI")]
        [SerializeField] private ResourcePanel _resourcePanel;
        
        [SerializeField] private UIButton _heroUpgradeButton;
        [SerializeField] private UIButton _backButton;

        [Space] 
        [SerializeField] private TMP_Text _costTMP;
        [SerializeField] private TMP_Text _heroLevelTMP;

        [Header("Dev Buttons")] 
        [SerializeField] private UIButton _devUpgradeButton;
        
        private MainMenuRoot _root;
        private PlayerUpgrade _playerUpgrade;
        private ISaveService _saveService;

        #region MonoBehaviour

        private void OnEnable()
        {
            _heroUpgradeButton.OnClicked += UpgradeHero;
            _backButton.OnClicked += ToMainMenu;

            _devUpgradeButton.OnClicked += ForceUpgrade;
            
            UpdateDisplayedData();
            ValidateButton();
        }

        private void OnDisable()
        {
            _heroUpgradeButton.OnClicked -= UpgradeHero;
            _backButton.OnClicked -= ToMainMenu;
            
            _devUpgradeButton.OnClicked -= ForceUpgrade;
        }

        private void Start() => 
            UpdateDisplayedData();

        #endregion

        [Inject]
        private void Construct(PlayerUpgrade playerUpgrade, ISaveService saveService)
        {
            _playerUpgrade = playerUpgrade;
            _saveService = saveService;
        }
        
        public void InitializeRoot(MainMenuRoot root) =>
            _root = root;

        private void ValidateButton() => 
            _heroUpgradeButton.OriginalButton.interactable = _playerUpgrade.CanUpgradeHero;

        private void UpdateDisplayedData()
        {
            _resourcePanel.DisplayResources();
            
            _costTMP.SetText(string.Format(CostFormat, _playerUpgrade.Cost));
            _heroLevelTMP.SetText(string.Format(HeroLevelFormat, _playerUpgrade.HeroLevel));
        }

        private void UpgradeHero()
        {
            _playerUpgrade.UpgradeHero();
            _saveService.Save();
            
            UpdateDisplayedData();
            
            ValidateButton();
        }

        private void ForceUpgrade()
        {
            _playerUpgrade.UpgradeHero(true);
            _saveService.Save();
            
            UpdateDisplayedData();
            
            ValidateButton();
        }

        private void ToMainMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowMainMenu();
        }
    }
}