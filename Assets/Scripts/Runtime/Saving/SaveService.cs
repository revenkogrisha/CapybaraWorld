using System.Collections.Generic;
using Zenject;

namespace Core.Saving
{
    // Save-load workflow is simplified due to MVP version & project size. Final version can differ.
    public class SaveService : ISaveService
    {
        private readonly ISaveSystem _saveSystem;
        private readonly List<ISaveable> _saveables = new();

        [Inject]
        public SaveService(ISaveSystem saveSystem) => 
            _saveSystem = saveSystem;

        public void Register(ISaveable saveable) =>
            _saveables.Add(saveable);

        public void ResetProcess()
        {
            SaveData emptyData = new();
            _saveSystem.Save(emptyData);

            Load();
        }

        public void Save()
        {
            SaveData data = new();
            foreach (ISaveable saveable in _saveables)
                saveable.Save(data);
            
            _saveSystem.Save(data);
        }

        public void Load()
        {
            SaveData data = _saveSystem.Load();

            foreach (ISaveable saveable in _saveables)
                saveable.Load(data);
        }
    }
}