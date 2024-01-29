namespace Core.Saving
{
    public interface ISaveSystem
    {
        public void Save(SaveData data);

        public SaveData Load();
    }
}