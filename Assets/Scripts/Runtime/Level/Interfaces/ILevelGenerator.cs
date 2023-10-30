using Core.Player;

namespace Core.Level
{
    public interface ILevelGenerator
    {
        public void Generate();

        public void ObservePlayer(PlayerTest hero);
    }
}
