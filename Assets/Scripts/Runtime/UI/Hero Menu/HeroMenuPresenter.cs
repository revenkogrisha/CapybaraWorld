using System;
using Core.Player;
using UnityEngine;

namespace Core.UI
{
    public class HeroMenuPresenter
    {
        private readonly PlayerUpgrade _playerUpgrade;
        private readonly HeroSkins _heroSkins;
        private readonly HeroMenuView _view;

        public HeroMenuPresenter(
            PlayerUpgrade playerUpgrade, 
            HeroSkins heroSkins, 
            HeroMenuView view)
        {
            _playerUpgrade = playerUpgrade;
            _heroSkins = heroSkins;
            _view = view;
        }

        public void OnViewReveal()
        {
            _view.InitializeSkins(_heroSkins.Current, _heroSkins.GetSortedPresets());

            if (_playerUpgrade.IsCostIncreased == true)
                _view.TryEnableRewardedAdButton();
            else
                _view.DisableRewardedAdButton();
        }

        public SkinPreset GetPresetByName(SkinName skinName) => 
            _heroSkins.GetByName(skinName);

        public void OnSkinBuyButtonClicked(SkinPreset skin)
        {
            _heroSkins.Buy(skin);
            SetPanelsByAvailability(skin.Name);
        }

        public void OnSelectButtonClicked(SkinPreset skin)
        {
            _heroSkins.SetCurrent(skin);
            SetPanelsByAvailability(skin.Name);
        }

        public void SetPanelsByAvailability(SkinName skinName)
        {
            switch (_heroSkins.GetAvailability(skinName))
            {
                case SkinAvailability.Buyable:
                    _view.SetSkinBuyable(_heroSkins.CanBuy(_view.GetDisplayedSkin()));
                    break;

                case SkinAvailability.Selectable:
                    _view.SetSkinSelectableState();
                    break;

                case SkinAvailability.Selected:
                    _view.SetSkinSelectedState();
                    break;

                default:
                    _view.SetSkinSelectedState();
                    break;
            }
        }

        public void ResetUpgradeCost() => 
            _playerUpgrade.ResetCost();

        public void UpgradeHero(bool force = false) => 
            _playerUpgrade.UpgradeHero(force);

        public void OnUpdateLevelData() => 
            _view.UpdateLevelData(_playerUpgrade.Cost, _playerUpgrade.HeroLevel);

        public void ValidateUpgradeButton() =>
            _view.UpdateUpgradeButton(_playerUpgrade.CanUpgradeHero);
    }
}