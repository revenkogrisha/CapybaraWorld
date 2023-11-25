using System;
using Core.Infrastructure;
using Core.Player;
using Cysharp.Threading.Tasks;
using UniRx;

namespace Core.Game
{
    public class GameFinishHandler : IDisposable
    {
        private readonly IGlobalStateMachine _globalStateMachine;
        private readonly CompositeDisposable _disposable;

        public GameFinishHandler(IGlobalStateMachine globalStateMachine)
        {
            _disposable = new();
            _globalStateMachine = globalStateMachine;
        }

        public void SubscribeHeroDeath(IDieable hero)
        {
            hero.IsDead
                .Where(isDead => isDead == true)
                .Subscribe(_ => FinishGame(GameResult.Lost))
                .AddTo(_disposable);
        }

        public void Dispose() => 
            _disposable.Clear();

        private void FinishGame(GameResult result)
        {
            switch (result)
            {
                case GameResult.Win:
                    OnGameWon();
                    break;

                case GameResult.Lost:
                    OnGameLost();
                    break;

                default:
                    throw new ArgumentException("Unknown GameResult value was gotten!");
            }
        }

        private void OnGameWon()
        {
            _globalStateMachine.ChangeState<GameWinState>();
            _disposable?.Clear();
        }

        private void OnGameLost()
        {
            _globalStateMachine.ChangeState<GameOverState>();
            _disposable?.Clear();
        }
    }
}