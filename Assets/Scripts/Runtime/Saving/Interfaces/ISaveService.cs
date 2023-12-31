namespace Core.Saving
{
    public interface ISaveService
    {
        public void Register(ISaveable saveable);
        public void ResetProcess();
        public void Save();
        public void Load();
    }
}