using System.Collections.Generic;
using System.Threading;
using Core.Mediation;
using Core.Other;
using Core.Player;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.UI
{
    public class HeroMenu : AnimatedUI, IAdRewardWaiter
    {
        private const string CostFormat = "{0}";
        private const string LevelFormat = "Common Hero Level: <b><color=#D978E9>{0}</color></b>";
        
        [Header("Resources")]
        [SerializeField] private ResourcePanel _resourcePanel;

        [Header("Skins Components")]
        [SerializeField] private SkinsPanel _skinsPanel;
        [SerializeField] private SkinPlacementPanel _skinPlacement;
        
        [Header("General UI")]
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
        
        private readonly CompositeDisposable _disposable = new();
        private MainMenuRoot _root;
        private IMediationService _mediationService;
        private PlayerUpgrade _playerUpgrade;
        private HeroMenuPresenter _skinsPresenter;
        private CancellationTokenSource _rewardedAdCTS;
        private SkinPreset _displayedSkin;
        private SkinPreset _selectedSkin;

        #region MonoBehaviour

        private void OnEnable()
        {
            _heroUpgradeButton.OnClicked += OnUpgradeButtonClicked;
            _backButton.OnClicked += ToMainMenu;
            _resetCostAdButton.OnClicked += ShowAdToResetCost;
            
            _skinsPanel.ItemDisplayCommand
                .Subscribe(DisplayItem)
                .AddTo(_disposable);

            _skinPlacement.BuyButtonCommand
                .Subscribe(_ => OnBuyButtonClicked())
                .AddTo(_disposable);

            _skinPlacement.SelectButtonCommand
                .Subscribe(_ => OnSelectButtonClicked())
                .AddTo(_disposable);

            UpdateView();

            if (_playerUpgrade.IsCostIncreased == true)
            {
                _resetCostAdButton.SetActive(_mediationService.IsRewardedAvailable);
                if (_mediationService.IsRewardedAvailable == false)
                    LoadRewardedAds().Forget();
            }
            else
            {
                _resetCostAdButton.SetActive(false);
            }

#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked += ForceUpgrade;
#endif
        }

        private void OnDisable()
        {
            FinilizeAdsCTS();

            _heroUpgradeButton.OnClicked -= OnUpgradeButtonClicked;
            _backButton.OnClicked -= ToMainMenu;
            _resetCostAdButton.OnClicked -= ShowAdToResetCost;
            
            _disposable.Clear();
            
#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked -= ForceUpgrade;
#endif
        }

        #endregion

        [Inject]
        private void Construct(IMediationService mediationService, PlayerUpgrade playerUpgrade)
        {
            _mediationService = mediationService;
            _playerUpgrade = playerUpgrade;
        }

        public override UniTask Reveal(CancellationToken token = default, bool enable = false)
        {
            _skinsPresenter.OnViewEnabled();
            _skinsPresenter.SetPanelsByAvailability(_displayedSkin.Name);
            
            return base.Reveal(token, enable);
        }

        public void OnRewardGranted()
        {
            _playerUpgrade.ResetCost();
            UpdateView();
        }

        public void Initialize(MainMenuRoot root, HeroMenuPresenter skinsPresenter)
        {
            _root = root;
            _skinsPresenter = skinsPresenter;
        }

        public void InitializeSkins(SkinPreset current, IEnumerable<SkinPreset> presets)
        {
            _skinsPanel.CreateItems(presets);

            _displayedSkin = current;
            _skinsPanel.CommandItemDisplay(current.Name);
        }

        public void SetSkinBuyable(bool canBuy) => 
            _skinPlacement.SetBuyState(_displayedSkin.FoodCost, canBuy);

        public void SetSkinSelectableState() => 
            _skinPlacement.SetSelectableState();

        public void SetSkinSelectedState()
        {
            _skinPlacement.SetSelectedState();

            if (_selectedSkin != null)
                _skinsPanel.SetSelected(_selectedSkin.Name, false);

            _selectedSkin = _displayedSkin;

            _skinsPanel.SetSelected(_displayedSkin.Name, true);
        }

        public SkinPreset GetDisplayedSkin() =>
            _displayedSkin;

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
            UpdateLevelData();
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

            FinilizeAdsCTS();
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

        private void UpdateLevelData()
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

        private void FinilizeAdsCTS()
        {
            _rewardedAdCTS.Clear();
            _rewardedAdCTS = null;
        }

        private void DisplayItem(SkinName name)
        {
            SkinPreset preset = _skinsPresenter.GetPresetByName(name);
            _displayedSkin = preset;
            
            _skinPlacement.DisplayPreset(preset);

            _skinsPresenter.SetPanelsByAvailability(name);
        }

        private void OnBuyButtonClicked()
        {
            _skinsPresenter.OnBuyButtonClicked(_displayedSkin);
            _resourcePanel.DisplayResources();
        }

        private void OnSelectButtonClicked()
        {
            _skinsPresenter.OnSelectButtonClicked(_displayedSkin);
        }
    }
}