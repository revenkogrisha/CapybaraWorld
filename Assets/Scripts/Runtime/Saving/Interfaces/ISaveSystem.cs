namespace Core.Saving
{
    public interface ISaveSystem
    {
        void Save(SaveData data);

        SaveData Load();
    }
}