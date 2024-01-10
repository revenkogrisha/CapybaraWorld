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
        private SkinPreset _current;

        public SkinPreset[] Presets => _presets;
        public SkinPreset Current => _current;

        private SkinName _boughtSkins;

        [Inject]
        public HeroSkins(ISaveService saveService, PlayerData playerData, SkinPreset[] presets)
        {
            _saveService = saveService;
            _playerData = playerData;
            _presets = presets;
            _current = GetByName(SkinName.Capy);
        }

        public void Save(SaveData data)
        {

        }

        public void Load(SaveData data)
        {

        }

        public void SetCurrent(SkinPreset preset) => 
            _current = preset;

        public void Buy(SkinPreset preset)
        {
            _boughtSkins |= preset.Name;

            _playerData.RemoveFood(preset.FoodCost);

            _saveService.Save();
        }

        public SkinAvailability GetAvailability(SkinName skinName)
        {
            if (skinName != _boughtSkins)
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