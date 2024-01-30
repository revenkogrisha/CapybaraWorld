using Core.Player;
using Core.Saving;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class HeroMenu : AnimatedUI
    {
        private const string CostFormat = "{0}";
        private const string LevelFormat = "Common Hero Level: <b><color=#D978E9>{0}</color></b>";
        
        [Header("Skins UI")]
        [SerializeField] private SkinsPanel _skinsPanel;
        [SerializeField] private SkinPlacementPanel _skinPlacement;
        
        [Header("Upgrade UI")]
        [SerializeField] private ResourcePanel _resourcePanel;
        
        [SerializeField] private UIButton _heroUpgradeButton;
        [SerializeField] private UIButton _backButton;

        [Space] 
        [SerializeField] private TMP_Text _costTMP;
        [SerializeField] private TMP_Text _heroLevelTMP;

        [Space]
        [SerializeField] private Color _costAvailableColor = Color.white;
        [SerializeField] private Color _costLockedColor = Color.red;
        
        [Header("Dev Buttons")] 
        [SerializeField] private UIButton _devUpgradeButton;
        
        private MainMenuRoot _root;
        private PlayerUpgrade _playerUpgrade;
        private ISaveService _saveService;
        private HeroSkins _heroSkins;
        private SkinsMenuPresenter _skinsPresenter;

        #region MonoBehaviour

        private void OnEnable()
        {
            _skinsPanel.CreateItems(_heroSkins.GetSortedPresets());
            _skinsPresenter.Enable();
            _skinsPanel.InvokeItemDisplay(_heroSkins.Current.Name);

            _heroUpgradeButton.OnClicked += UpgradeHero;
            _backButton.OnClicked += ToMainMenu;

#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked += ForceUpgrade;
#endif

            UpdateView();
        }

        private void OnDisable()
        {
            _skinsPresenter.Disable();

            _heroUpgradeButton.OnClicked -= UpgradeHero;
            _backButton.OnClicked -= ToMainMenu;
            
#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked -= ForceUpgrade;
#endif
        }

        #endregion

        [Inject]
        private void Construct(PlayerUpgrade playerUpgrade,
            ISaveService saveService,
            HeroSkins heroSkins)
        {
            _playerUpgrade = playerUpgrade;
            _saveService = saveService;
            _heroSkins = heroSkins;

            _skinsPresenter = new(_heroSkins, _skinsPanel, _skinPlacement, _resourcePanel);
        }
        
        public void InitializeRoot(MainMenuRoot root) =>
            _root = root;

        private void UpgradeHero()
        {
            _playerUpgrade.UpgradeHero();
            
            UpdateView();
        }

        private void ForceUpgrade()
        {
            _playerUpgrade.UpgradeHero(true);
            
            UpdateView();
        }

        private void UpdateView()
        {
            UpdateDisplayedData();
            ValidateButton();
        }

        private void ToMainMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowMainMenu();
        }

        private void UpdateDisplayedData()
        {
            _resourcePanel.DisplayResources();
            
            _costTMP.SetText(string.Format(CostFormat, _playerUpgrade.Cost));
            _heroLevelTMP.SetText(string.Format(LevelFormat, _playerUpgrade.HeroLevel));
        }

        private void ValidateButton()
        {
            _heroUpgradeButton.Interactable = _playerUpgrade.CanUpgradeHero;
            if (_playerUpgrade.CanUpgradeHero == true)
                _costTMP.color = _costAvailableColor;
            else
                _costTMP.color = _costLockedColor;
        }
    }
}