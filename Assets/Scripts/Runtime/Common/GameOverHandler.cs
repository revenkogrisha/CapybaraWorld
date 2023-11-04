using System;
using Core.Infrastructure;
using Core.Player;

namespace Core.Common
{
    public class GameOverHandler : IDisposable
    {
        private readonly IGlobalStateMachine _globalStateMachine;
        private Hero _hero;

        public GameOverHandler(IGlobalStateMachine globalStateMachine) =>
            _globalStateMachine = globalStateMachine;

        public void SubscribeHeroDeath(Hero hero)
        {
            _hero = hero;
            _hero.Died += FinishGame;
        }

        public void Dispose()
        {
            if (_hero != null)
                _hero.Died -= FinishGame;
        }

        private void FinishGame()
        {
            _hero.Died -= FinishGame;
            _globalStateMachine.ChangeState<GameOverState>();
        }
    }
}
