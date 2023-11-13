using System;

namespace Core.Game
{
    public interface IScoreCounter : IDisposable
    {
        public void StartCount();

        public void StopCount();
    }
}