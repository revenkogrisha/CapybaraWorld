using System.Linq;
using Core.Common.ThirdParty;
using Core.Saving;
using Firebase.Analytics;
using Zenject;

namespace Core.Player
{
    public class HeroSkins : ISaveable
    {
        private readonly ISaveService _saveService;
        private readonly PlayerData _playerData;
        private readonly SkinsCollection _skins;
        private SkinName _boughtSkins;

        public SkinPreset Current { get; private set; }

        [Inject]
        public HeroSkins(ISaveService saveService, PlayerData playerData, SkinsCollection skins)
        {
            _saveService = saveService;
            _playerData = playerData;
            _skins = skins;
        }

        public void Save(SaveData data)
        {
            data.BoughtHeroSkins = _boughtSkins;
            data.CurrentHeroSkin = Current.Name;
        }

        public void Load(SaveData data)
        {
            _boughtSkins = data.BoughtHeroSkins;
            Current = GetByName(data.CurrentHeroSkin);
        }

        public void SetCurrent(SkinPreset preset)
        {
            Current = preset;
            _saveService.Save();
        }

        public SkinPreset[] GetSortedPresets()
        {
            return _skins.Presets
                .OrderByDescending(item => item.Name == Current.Name)
                .ThenByDescending(item => _boughtSkins.HasFlag(item.Name) == true)
                .ThenBy(item => item.FoodCost)
                .ToArray();
        }

        public void Buy(SkinPreset preset)
        {
            _boughtSkins |= preset.Name;

            _playerData.RemoveFood(preset.FoodCost);

            _saveService.Save();

            FirebaseService.LogEvent(EventName.SkinBought,
                new Parameter(ParameterName.Skin.ToString(), preset.Name.ToString()),
                new Parameter(ParameterName.HaveSkins.ToString(), _boughtSkins.ToString())
            );
        }

        public SkinAvailability GetAvailability(SkinName skinName)
        {
            if (_boughtSkins.HasFlag(skinName) == false)
                return SkinAvailability.Buyable;
            else if (skinName == Current.Name)
                return SkinAvailability.Selected;
            else
                return SkinAvailability.Selectable;
        }

        public bool CanBuy(SkinPreset preset) =>
            preset.FoodCost <= _playerData.FoodAmount;

        public SkinPreset GetByName(SkinName skinName) => 
            _skins.Presets.Single(preset => preset.Name == skinName);
    }
}