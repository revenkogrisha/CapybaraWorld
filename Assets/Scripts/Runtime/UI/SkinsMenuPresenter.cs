using Core.Player;
using UniRx;

namespace Core.UI
{
    public class SkinsMenuPresenter
    {
        private readonly HeroSkins _heroSkins;
        private readonly SkinsPanel _panel;
        private readonly SkinPlacementPanel _placement;
        private readonly ResourcePanel _resourcePanel;
        private readonly CompositeDisposable _disposable = new();
        private SkinPreset _displayedPreset;
        private SkinPreset _selectedPreset;

        public SkinsMenuPresenter(
            HeroSkins heroSkins,
            SkinsPanel panel,
            SkinPlacementPanel placement,
            ResourcePanel resourcePanel)
        {
            _heroSkins = heroSkins;
            _panel = panel;
            _placement = placement;
            _resourcePanel = resourcePanel;
        }

        public void Enable()
        {
            _panel.ItemDisplayCommand
                .Subscribe(skinName => DisplayItem(skinName))
                .AddTo(_disposable);

            _placement.BuyButtonCommand
                .Subscribe(_ => OnBuyButtonClicked())
                .AddTo(_disposable);

            _placement.SelectButtonCommand
                .Subscribe(_ => OnSelectButtonClicked())
                .AddTo(_disposable);
        }

        public void Disable() =>
            _disposable.Clear();

        private void DisplayItem(SkinName skinName)
        {
            SkinPreset preset = _heroSkins.GetByName(skinName);
            _displayedPreset = preset;

            _placement.DisplayPreset(preset);
            SetPanelsByAvailability(preset);
        }

        private void SetPanelsByAvailability(SkinPreset preset)
        {
            switch (_heroSkins.GetAvailability(preset.Name))
            {
                case SkinAvailability.Buyable:
                    _placement.SetBuyState(preset.FoodCost, _heroSkins.CanBuy(_displayedPreset));
                    break;

                case SkinAvailability.Selectable:
                    _placement.SetSelectableState();
                    break;

                case SkinAvailability.Selected:
                    _placement.SetSelectedState();

                    if (_selectedPreset != null)
                        _panel.SetSelected(_selectedPreset.Name, false);

                    _selectedPreset = _displayedPreset;

                    _panel.SetSelected(_displayedPreset.Name, true);
                    break;

                default:
                    _placement.SetSelectedState();
                    break;
            }
        }

        private void OnBuyButtonClicked()
        {
            _heroSkins.Buy(_displayedPreset);
            _resourcePanel.DisplayResources();
            
            SetPanelsByAvailability(_displayedPreset);
        }

        private void OnSelectButtonClicked()
        {
            _heroSkins.SetCurrent(_displayedPreset);
            SetPanelsByAvailability(_displayedPreset);
        }
    }
}