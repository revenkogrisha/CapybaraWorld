using System;
using Core.Infrastructure;
using Core.Player;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using Zenject;

namespace Core.Game
{
    public class PlaythroughHandler : IPlaythroughProgressHandler, IGameEventsHandler, IDisposable
    {
        private const int WinScore = 130;
        
        private readonly IGlobalStateMachine _globalStateMachine;
        private readonly Score _score;
        private readonly CompositeDisposable _disposable = new();

        public ReactiveCommand GameWinCommand { get; } = new();
        public float PlaythroughProgress => (float)_score.PlaythroughScore.Value / (float)WinScore;

        [Inject]
        public PlaythroughHandler(IGlobalStateMachine globalStateMachine, Score score)
        {
            _globalStateMachine = globalStateMachine;
            _score = score;
        }

        public void Initialize(IDieable hero)
        {
            _score.PlaythroughScore
                .Where(value => value >= WinScore)
                .Subscribe(_ => FinishGame(GameResult.Win))
                .AddTo(_disposable);
            
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
            GameWinCommand.Execute();
            _disposable?.Clear();
        }

        private void OnGameLost()
        {
            _globalStateMachine.ChangeState<GameLostState>();
            _disposable?.Clear();
        }
    }
}