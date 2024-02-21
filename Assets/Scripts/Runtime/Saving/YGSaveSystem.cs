using System.Text;
using Core.Editor.Debugger;
using YG;

namespace Core.Saving
{
    public class YGSaveSystem : ISaveSystem
    {
        public void Save(SaveData data)
        {
            YandexGame.savesData.SaveDataBytes = Encoding.UTF8.GetBytes(
                JsonSaveSystem.Serialize(data)
            );

            RDebug.Info("SaveDataJSON: " + YandexGame.savesData.SaveDataBytes);

            YandexGame.SaveProgress();
        }

        public SaveData Load()
        {
            if (YandexGame.savesData.SaveDataBytes == null)
                return new SaveData();
            
            RDebug.Info("SaveDataJSON: " + YandexGame.savesData.SaveDataBytes);
            return JsonSaveSystem.Deserialize(
                Encoding.UTF8.GetString(YandexGame.savesData.SaveDataBytes)
            );
        }
    }
}