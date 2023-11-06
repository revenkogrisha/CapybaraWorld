using System;
using Core.Infrastructure;
using Core.Player;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Core.Common
{
    public class GameOverHandler : IDisposable
    {
        private readonly IGlobalStateMachine _globalStateMachine;
        private readonly CompositeDisposable _finishDisposable;

        public GameOverHandler(IGlobalStateMachine globalStateMachine)
        {
            _finishDisposable = new();
            _globalStateMachine = globalStateMachine;
        }

        public void SubscribeHeroDeath(Hero hero)
        {
            hero.IsDead
                .Where(isDead => isDead == true)
                .Subscribe(FinishGame)
                .AddTo(_finishDisposable);
        }

        public void Dispose() => 
            _finishDisposable.Clear();

        private void FinishGame(bool _)
        {
            _globalStateMachine.ChangeState<GameOverState>();
            _finishDisposable?.Clear();
        }
    }
}
