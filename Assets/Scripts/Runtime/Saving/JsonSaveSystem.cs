using System.IO;
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
            string json = JsonConvert.SerializeObject(data);

            using StreamWriter writer = new(FilePath);
            writer.Write(json);
        }

        public SaveData Load()
        {
            if (File.Exists(FilePath) == false)
                return new SaveData();

            string json = "";
            using StreamReader reader = new(FilePath);

            string line = "";
            while ((line = reader.ReadLine()) != null)
                json += line;

            if (string.IsNullOrEmpty(json) == true)
                return new SaveData();
            
            return JsonConvert.DeserializeObject<SaveData>(json);
        }
    }
}