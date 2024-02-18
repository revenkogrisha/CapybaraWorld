using YG;

namespace Core.Saving
{
    public class YGSaveSystem : ISaveSystem
    {
        public void Save(SaveData data) => 
            YandexGame.savesData.SaveDataJSON = JsonSaveSystem.Serialize(data);

        public SaveData Load() => 
            JsonSaveSystem.Deserialize(YandexGame.savesData.SaveDataJSON);
    }
}