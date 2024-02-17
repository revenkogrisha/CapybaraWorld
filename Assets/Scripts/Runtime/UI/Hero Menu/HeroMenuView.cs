using System.Collections.Generic;
using System.Threading;
using Core.Audio;
using Core.Mediation;
using Core.Other;
using Core.Player;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UnityEngine;
using YG;
using Zenject;

namespace Core.UI
{
    public class HeroMenuView : AnimatedUI, IAdRewardWaiter
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
        private IAudioHandler _audioHandler;
        private HeroMenuPresenter _presenter;
        private CancellationTokenSource _rewardedAdCTS;
        private SkinPreset _displayedSkin;
        private SkinPreset _selectedSkin;

        public int RewardID => 0;

        #region MonoBehaviour

        private void OnEnable()
        {
#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked += ForceUpgrade;
#endif

            YandexGame.RewardVideoEvent += OnRewardGranted;

            _heroUpgradeButton.OnClicked += OnUpgradeButtonClicked;
            _backButton.OnClicked += ToMainMenu;
            _resetCostAdButton.OnClicked += ShowAdToResetCost;
            
            _skinsPanel.ItemDisplayCommand
                .Subscribe(DisplayItem)
                .AddTo(_disposable);

            _skinPlacement.BuyButtonCommand
                .Subscribe(_ => OnSkinBuyButtonClicked())
                .AddTo(_disposable);

            _skinPlacement.SelectButtonCommand
                .Subscribe(_ => OnSelectButtonClicked())
                .AddTo(_disposable);
        }

        private void OnDisable()
        {
#if REVENKO_DEVELOP
            _devUpgradeButton.OnClicked -= ForceUpgrade;
#endif

            YandexGame.RewardVideoEvent -= OnRewardGranted;
            
            _heroUpgradeButton.OnClicked -= OnUpgradeButtonClicked;
            _backButton.OnClicked -= ToMainMenu;
            _resetCostAdButton.OnClicked -= ShowAdToResetCost;

            FinilizeAdsCTS();
            
            _disposable.Clear();
        }

        #endregion

        [Inject]
        private void Construct(
            IMediationService mediationService,
            IAudioHandler audioHandler)
        {
            _mediationService = mediationService;
            _audioHandler = audioHandler;
        }

        public override UniTask Reveal(CancellationToken token = default, bool enable = false)
        {
            _presenter.OnViewReveal();
            _presenter.SetPanelsByAvailability(_displayedSkin.Name);

            DisplayItem(_displayedSkin.Name);
            UpdateView();
            
            return base.Reveal(token, enable);
        }

        public void OnRewardGranted(int id)
        {
            if (id == RewardID)
            {
                _presenter.ResetUpgradeCost();
                UpdateView();
            }
        }

        public void Initialize(MainMenuRoot root, HeroMenuPresenter presenter)
        {
            _root = root;
            _presenter = presenter;
        }

        public void InitializeSkins(SkinPreset current, IEnumerable<SkinPreset> presets)
        {
            _skinsPanel.CreateItems(presets);

            _displayedSkin = current;
            _skinsPanel.CommandItemDisplay(current.Name);
        }

        public void TryEnableRewardedAdButton()
        {
            _resetCostAdButton.SetActive(_mediationService.IsRewardedAvailable);
            if (_mediationService.IsRewardedAvailable == false)
                LoadRewardedAds().Forget();
        }

        public void DisableRewardedAdButton() =>
            _resetCostAdButton.SetActive(false);

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

        public void UpdateLevelData(int cost, int heroLevel)
        {
            _costTMP.SetText(string.Format(CostFormat, cost));
            _heroLevelTMP.SetText(string.Format(LevelFormat, heroLevel));
        }

        public void UpdateUpgradeButton(bool canUpgradeHero)
        {
            _heroUpgradeButton.Interactable = canUpgradeHero;

            if (canUpgradeHero == true)
                _costTMP.color = _costAvailableColor;
            else
                _costTMP.color = _costLockedColor;
        }

        private async void OnUpgradeButtonClicked()
        {
            _audioHandler.PlaySound(AudioName.CoinsSpent);
            
            UpgradeHero();

            await UniTaskUtility.Delay(1f, default);
            _mediationService.ShowInterstitial();
        }

        private void ForceUpgrade()
        {
            _presenter.UpgradeHero(true);
            
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
            _presenter.UpgradeHero();
            
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
            
            _presenter.OnUpdateLevelData();
        }

        private void ValidateUpgradeButton() => 
            _presenter.ValidateUpgradeButton();

        private void FinilizeAdsCTS()
        {
            _rewardedAdCTS.Clear();
            _rewardedAdCTS = null;
        }

        private void DisplayItem(SkinName name)
        {
            SkinPreset preset = _presenter.GetPresetByName(name);
            _displayedSkin = preset;
            
            _skinPlacement.DisplayPreset(preset);

            _presenter.SetPanelsByAvailability(name);
        }

        private void OnSkinBuyButtonClicked()
        {
            _audioHandler.PlaySound(AudioName.FoodBite);
            
            _presenter.OnSkinBuyButtonClicked(_displayedSkin);
            _resourcePanel.DisplayResources();
        }

        private void OnSelectButtonClicked() => 
            _presenter.OnSelectButtonClicked(_displayedSkin);
    }
}