using Core.Player;

namespace Core.UI
{
    public class HeroMenuPresenter
    {
        private readonly HeroSkins _heroSkins;
        private readonly HeroMenu _view;

        public HeroMenuPresenter(HeroSkins heroSkins, HeroMenu view)
        {
            _heroSkins = heroSkins;
            _view = view;
        }

        public void OnViewEnabled() => 
            _view.InitializeSkins(_heroSkins.Current, _heroSkins.GetSortedPresets());

        public SkinPreset GetPresetByName(SkinName skinName) => 
            _heroSkins.GetByName(skinName);

        public void OnBuyButtonClicked(SkinPreset skin)
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
    }
}