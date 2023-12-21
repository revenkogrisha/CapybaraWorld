namespace Core.Level
{
    public interface ICollectable
    {
        public bool CanCollect { get; }
        
        public void OnCollected();
    }
}