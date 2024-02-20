using YG;

namespace Core.Saving
{
    public class YGSaveSystem : ISaveSystem
    {
        public void Save(SaveData data)
        {
            YandexGame.savesData.SaveDataJSON = JsonSaveSystem.Serialize(data);
            YandexGame.SaveProgress();
        }

        public SaveData Load()
        {
            if (string.IsNullOrEmpty(YandexGame.savesData.SaveDataJSON) == true)
                return new SaveData();
                
            return JsonSaveSystem.Deserialize(YandexGame.savesData.SaveDataJSON);
        }
    }
}