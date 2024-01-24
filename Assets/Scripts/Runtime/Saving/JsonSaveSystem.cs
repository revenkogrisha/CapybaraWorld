using System;
using System.IO;
using System.Text;
using Core.Editor.Debugger;
using Newtonsoft.Json;

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
            try
            {
                JsonSerializerSettings serializerSettings = new()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };

                File.WriteAllText(FilePath,
                    JsonConvert.SerializeObject(data, Formatting.Indented, serializerSettings),
                    Encoding.UTF8);
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(JsonSaveSystem)}::{nameof(Save)} Failed to save data: {ex.Message} \n {ex.StackTrace}", true);
            }
        }

        public SaveData Load()
        {
            try
            {
                if (File.Exists(FilePath) == false)
                    return new SaveData();

                string json = File.ReadAllText(FilePath, Encoding.UTF8);

                if (string.IsNullOrEmpty(json) == true)
                    return new SaveData();
                
                return JsonConvert.DeserializeObject<SaveData>(json);
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(JsonSaveSystem)}::{nameof(Load)} Failed to load data: {ex.Message} \n {ex.StackTrace}", true);

                return new SaveData();
            }
        }
    }
}