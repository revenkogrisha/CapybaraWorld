using System;

namespace Core.Saving
{
    [Serializable]
    public class SaveData
    {
        public int CoinsAmount = 0;
        public int FoodAmount = 0;
        public int LevelNumber = 1;
        public int LocationIndex = 0;
        public int UpgradeCost = 15;
        public int HeroLevel = 1;
        public bool IsLocationRandomSelectionEnabled = false;
    }
}