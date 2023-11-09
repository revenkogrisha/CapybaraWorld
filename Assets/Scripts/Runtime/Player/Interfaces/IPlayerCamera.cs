using System;

namespace Core.Player
{
    public interface IPlayerCamera : IDisposable
    {
        public void Initialize(Hero hero);
    }
}
