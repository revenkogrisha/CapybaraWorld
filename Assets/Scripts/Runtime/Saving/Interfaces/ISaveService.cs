namespace Core.Saving
{
    public interface ISaveService
    {
        public const string SaveFileName = "CapybaraWorld";
        
        public void Register(ISaveable saveable);
        public void Save();
        public void Load();
    }
}