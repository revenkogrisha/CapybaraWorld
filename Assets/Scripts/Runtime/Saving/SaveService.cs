using System.Collections.Generic;
using Core.Editor.Debugger;
using Zenject;

namespace Core.Saving
{
    public class SaveService : ISaveService
    {
        private readonly ISaveSystem _saveSystem;
        private readonly List<ISaveable> _saveables = new();

        [Inject]
        public SaveService(ISaveSystem saveSystem) => 
            _saveSystem = saveSystem;

        public void Register(ISaveable saveable) =>
            _saveables.Add(saveable);

        public void Save()
        {
            SaveData data = new();
            foreach (ISaveable saveable in _saveables)
                saveable.Save(data);

            _saveSystem.Save(data);

            RDebug.Info($"{nameof(SaveService)}: Data was saved");
        }

        public void Load()
        {
            SaveData data = _saveSystem.Load();

            foreach (ISaveable saveable in _saveables)
                saveable.Load(data);
            
            RDebug.Info($"{nameof(SaveService)}: Data was loaded");
        }
    }
}