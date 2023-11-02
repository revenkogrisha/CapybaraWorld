using System;
using Core.Player;

namespace Core.Level
{
    public interface ILevelGenerator : IDisposable
    {
        public void Generate();

        public void ObservePlayer(PlayerTest hero);
    }
}
