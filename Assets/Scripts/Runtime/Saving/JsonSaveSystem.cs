using System.IO;
using UnityEngine;

namespace Core.Saving
{
    public class JsonSaveSystem : ISaveSystem
    {
        public const string SaveFileFormat = "/CapybaraWorld.json";

        private readonly string _filePath;

        public JsonSaveSystem() => 
            _filePath = Application.persistentDataPath + SaveFileFormat;

        public void Save(SaveData data)
        {
            string json = JsonUtility.ToJson(data);
            using StreamWriter writer = new(_filePath);

            writer.Write(json);
        }

        public SaveData Load()
        {
            if (File.Exists(_filePath) == false)
                return new SaveData();

            string json = "";
            using StreamReader reader = new(_filePath);

            string line = "";
            while ((line = reader.ReadLine()) != null)
                json += line;

            if (string.IsNullOrEmpty(json))
                return new SaveData();

            return JsonUtility.FromJson<SaveData>(json);
        }
    }
}