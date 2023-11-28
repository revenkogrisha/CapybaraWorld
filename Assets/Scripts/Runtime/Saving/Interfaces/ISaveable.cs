namespace Core.Saving
{
    public interface ISaveable
    {
        public void Save(SaveData data);
        public void Load(SaveData data);
    }
}