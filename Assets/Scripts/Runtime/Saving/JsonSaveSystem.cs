using System.IO;
using System.Text;
using Unity.Plastic.Newtonsoft.Json;

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Core.Saving
{
    public class JsonSaveSystem : ISaveSystem
    {
        public const string SaveFileFormat = "/CapybaraWorld.json";

        private static string FilePath = Application.persistentDataPath + SaveFileFormat;

        #region Editor

#if UNITY_EDITOR
        [MenuItem("Tools/CapybaraWorld/Delete Save Data", false, 20)]
        public static void DeleteSaveData()
        {
            if (EditorUtility.DisplayDialog("Delete Save Data", "Are you sure that you want to DELETE your SAVE DATA?",
                    "DELETE MY DATA!", "Cancel"))
            {
                if (File.Exists(FilePath) == false)
                {
                    EditorUtility.DisplayDialog("Failed", "You don't have save data, or the path is incorrect. You also maybe using the wrong 'SaveSystem'", "Okay");
                    return;
                }
                
                File.Delete(FilePath);
                PlayerPrefsUtility.ClearData();
                
                EditorUtility.DisplayDialog("Succeeded", "Your save data was deleted", "Okay");
            }
        }
#endif

        #endregion

        public void Save(SaveData data)
        {
            const int bufferSize = 2048;
            string json = JsonConvert.SerializeObject(data);

            using StreamWriter writer = new(FilePath, false, Encoding.UTF8, bufferSize);
            writer.Write(json);
        }

        public SaveData Load()
        {
            if (File.Exists(FilePath) == false)
                return new SaveData();

            string json = File.ReadAllText(FilePath, Encoding.UTF8);

            if (string.IsNullOrEmpty(json) == true)
                return new SaveData();
            
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
    }
}