using Core.Saving;

namespace Core.Level
{
    public interface ILocationsHandler : ISaveable
    {
        public Location CurrentLocation { get; }

        public void SetRandomLocation();
    }
}
