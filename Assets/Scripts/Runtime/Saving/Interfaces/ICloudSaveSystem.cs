namespace Core.Saving
{
    public interface ICloudSaveSystem 
    {
        public bool IsAvailable { get; }

        public void Save(SaveData data);

        public void LoadToLocal();
    }
}
