using Core.Player;
using UniRx;

namespace Core.UI
{
    public class SkinsMenuPresenter
    {
        private readonly HeroSkins _heroSkins;
        private readonly SkinsPanel _panel;
        private readonly SkinPlacementPanel _placement;
        private readonly CompositeDisposable _disposable = new();
        private SkinPreset _displayedPreset;

        public SkinsMenuPresenter(HeroSkins heroSkins,
            SkinsPanel panel,
            SkinPlacementPanel placement)
        {
            _heroSkins = heroSkins;
            _panel = panel;
            _placement = placement;
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
                    _panel.SetSelected(_displayedPreset.Name);
                    break;

                default:
                    _placement.SetSelectedState();
                    break;
            }
        }

        private void OnBuyButtonClicked() => 
            _heroSkins.Buy(_displayedPreset);

        private void OnSelectButtonClicked()
        {
            _heroSkins.SetCurrent(_displayedPreset);
            SetPanelsByAvailability(_displayedPreset);
        }
    }
}