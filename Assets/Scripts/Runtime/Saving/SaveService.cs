using System;
using System.Collections.Generic;
using Core.Editor.Debugger;
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
            try
            {
                SaveData data = new();
                foreach (ISaveable saveable in _saveables)
                    saveable.Save(data);

                _saveSystem.Save(data);

                RDebug.Info($"{nameof(SaveService)}: Data was saved");
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(SaveService)}::{nameof(Save)} Failed to save data: {ex.Message} \n {ex.StackTrace}", true);
            }
        }

        public void Load()
        {
            try
            {
                SaveData data = _saveSystem.Load();

                foreach (ISaveable saveable in _saveables)
                    saveable.Load(data);
                
                RDebug.Info($"{nameof(SaveService)}: Data was loaded");
            }
            catch (Exception ex)
            {
                RDebug.Error($"{nameof(SaveService)}::{nameof(Load)} Failed to load data: {ex.Message} \n {ex.StackTrace}", true);
            }
        }
    }
}