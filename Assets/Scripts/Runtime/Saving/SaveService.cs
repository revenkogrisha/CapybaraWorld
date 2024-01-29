using System.Collections.Generic;
using Core.Editor.Debugger;
using Zenject;

namespace Core.Saving
{
    public class SaveService : ISaveService
    {
        private readonly ISaveSystem _saveSystem;
        private readonly ICloudSaveSystem _cloudSaveSystem;
        private readonly List<ISaveable> _saveables = new();

        [Inject]
        public SaveService(ISaveSystem saveSystem, ICloudSaveSystem cloudSaveSystem)
        {
            _saveSystem = saveSystem;
            _cloudSaveSystem = cloudSaveSystem;
        }

        public void Register(ISaveable saveable) =>
            _saveables.Add(saveable);

        public void Save()
        {
            SaveData data = new();
            foreach (ISaveable saveable in _saveables)
                saveable.Save(data);

            _saveSystem.Save(data);

            if (_cloudSaveSystem.IsAvailable == true)
            {
                _cloudSaveSystem.Save(data);

                RDebug.Info($"{nameof(SaveService)}: Data was saved to cloud");
                return;
            }

            RDebug.Info($"{nameof(SaveService)}: Data was saved locally");
        }

        public void Load()
        {
            SaveData data = _saveSystem.Load();

            if (data.Equals(default) == true
                && _cloudSaveSystem.IsAvailable == true)
            {
                _cloudSaveSystem.LoadToLocal();
                data = _saveSystem.Load();

                RDebug.Info($"{nameof(SaveService)}: Data was loaded from cloud");
            }
            else
            {
                RDebug.Info($"{nameof(SaveService)}: Data was loaded from local");
            }

            foreach (ISaveable saveable in _saveables)
                saveable.Load(data);
        }
    }
}