using System.IO;
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
                EditorUtility.DisplayDialog("Succeeded", "Your save data was deleted", "Okay");
            }
        }
#endif

        public void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
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

            if (string.IsNullOrEmpty(json))
                return new SaveData();
            
            return JsonUtility.FromJson<SaveData>(json);
        }
    }
}