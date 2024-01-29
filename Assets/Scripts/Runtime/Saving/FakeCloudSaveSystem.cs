using Core.Editor.Debugger;

namespace Core.Saving
{
    public class FakeCloudSaveSystem : ICloudSaveSystem
    {
        public bool IsAvailable => true;

        public void Save(SaveData data) => 
            RDebug.Log($"Fake save to cloud");

        public void LoadToLocal() =>
            RDebug.Log($"Fake load to local");
    }
}
