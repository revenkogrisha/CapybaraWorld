using System.Linq;
using Core.Saving;
using Zenject;

namespace Core.Player
{
    public class HeroSkins : ISaveable
    {
        private readonly ISaveService _saveService;
        private readonly PlayerData _playerData;
        private readonly SkinPreset[] _presets;
        private SkinName _boughtSkins;

        public SkinPreset Current { get; private set; }

        [Inject]
        public HeroSkins(ISaveService saveService, PlayerData playerData, SkinPreset[] presets)
        {
            _saveService = saveService;
            _playerData = playerData;
            _presets = presets;
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
            return _presets
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
            _presets.Single(preset => preset.Name == skinName);
    }
}