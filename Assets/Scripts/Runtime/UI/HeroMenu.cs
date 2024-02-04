using System.Threading;
using Core.Mediation;
using Core.Other;
using Core.Player;
using Core.Saving;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class HeroMenu : AnimatedUI, IAdRewardWaiter
    {
        private const string CostFormat = "{0}";
        private const string LevelFormat = "Common Hero Level: <b><color=#D978E9>{0}</color></b>";
        
        [Header("Skins UI")]
        [SerializeField] private SkinsPanel _skinsPanel;
        [SerializeField] private SkinPlacementPanel _skinPlacement;
        
        [Header("Upgrade UI")]
        [SerializeField] private ResourcePanel _resourcePanel;
        
        [SerializeField] private UIButton _heroUpgradeButton;
        [SerializeField] private UIButton _resetCostAdButton;
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
        private IMediationService _mediationService;
        private PlayerUpgrade _playerUpgrade;
        private HeroSkins _heroSkins;
        private SkinsMenuPresenter _skinsPresenter;
        private CancellationTokenSource _rewardedAdCTS;

        #region MonoBehaviour

        private void OnEnable()
        {
            _skinsPanel.CreateItems(_heroSkins.GetSortedPresets());
            _skinsPresenter.Enable();
            _skinsPanel.InvokeItemDisplay(_heroSkins.Current.Name);

            _heroUpgradeButton.OnClicked += OnUpgradeButtonClicked;
            _backButton.OnClicked += ToMainMenu;
            _resetCostAdButton.OnClicked += ShowAdToResetCost;

#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked += ForceUpgrade;
#endif

            UpdateView();

            _resetCostAdButton.SetActive(_mediationService.IsRewardedAvailable);
            if (_mediationService.IsRewardedAvailable == false)
                LoadRewardedAds().Forget();
        }

        private void OnDisable()
        {
            _skinsPresenter.Disable();

            _rewardedAdCTS.Clear();
            _rewardedAdCTS = null;

            _heroUpgradeButton.OnClicked -= OnUpgradeButtonClicked;
            _backButton.OnClicked -= ToMainMenu;
            _resetCostAdButton.OnClicked -= ShowAdToResetCost;
            
#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked -= ForceUpgrade;
#endif
        }

        #endregion

        [Inject]
        private void Construct(
            IMediationService mediationService,
            PlayerUpgrade playerUpgrade,
            HeroSkins heroSkins)
        {
            _mediationService = mediationService;
            _playerUpgrade = playerUpgrade;
            _heroSkins = heroSkins;

            _skinsPresenter = new(_heroSkins, _skinsPanel, _skinPlacement, _resourcePanel);
        }

        public void OnRewardGranted()
        {
            _playerUpgrade.ResetCost();
            UpdateView();
        }

        public void InitializeRoot(MainMenuRoot root) =>
            _root = root;

        private void OnUpgradeButtonClicked()
        {
            UpgradeHero();
            _mediationService.ShowInterstitial();
        }

        private void ForceUpgrade()
        {
            _playerUpgrade.UpgradeHero(true);
            
            UpdateView();
        }

        private void UpdateView()
        {
            UpdateDisplayedData();
            ValidateUpgradeButton();
        }

        private async UniTaskVoid LoadRewardedAds()
        {
            const float loadTimeout = 30f;
            
            _rewardedAdCTS.Clear();
            _rewardedAdCTS = new();

            _mediationService.LoadRewarded();

            _rewardedAdCTS.CancelByTimeout(loadTimeout).Forget();
            
            await UniTask.WaitUntil(
                () => _mediationService.IsRewardedAvailable == true, 
                cancellationToken: _rewardedAdCTS.Token);

            _resetCostAdButton.SetActive(_mediationService.IsRewardedAvailable);

            _rewardedAdCTS.Clear();
            _rewardedAdCTS = null;
        }

        private void ToMainMenu()
        {
            Conceal(disable: true).Forget();
            _root.ShowMainMenu();
        }

        private void UpgradeHero()
        {
            _playerUpgrade.UpgradeHero();
            
            UpdateView();
        }

        private void ShowAdToResetCost()
        {
            _resetCostAdButton.SetActive(false);
            _mediationService.ShowRewarded(this);
        }

        private void UpdateDisplayedData()
        {
            _resourcePanel.DisplayResources();
            
            _costTMP.SetText(string.Format(CostFormat, _playerUpgrade.Cost));
            _heroLevelTMP.SetText(string.Format(LevelFormat, _playerUpgrade.HeroLevel));
        }

        private void ValidateUpgradeButton()
        {
            _heroUpgradeButton.Interactable = _playerUpgrade.CanUpgradeHero;
            if (_playerUpgrade.CanUpgradeHero == true)
                _costTMP.color = _costAvailableColor;
            else
                _costTMP.color = _costLockedColor;
        }
    }
}