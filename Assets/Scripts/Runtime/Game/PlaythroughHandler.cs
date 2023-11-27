using System;
using Core.Infrastructure;
using Core.Player;
using Cysharp.Threading.Tasks;
using UniRx;
using Zenject;

namespace Core.Game
{
    public class PlaythroughHandler : IPlaythroughProgressHandler, IGameEventsHandler, IDisposable
    {
        private const int WinScore = 130;
        
        private readonly GameNavigation _navigation;
        private readonly Score _score;
        private readonly CompositeDisposable _disposable = new();

        public ReactiveCommand GameWinCommand { get; } = new();
        public float PlaythroughProgress => (float)_score.PlaythroughScore.Value / (float)WinScore;

        [Inject]
        public PlaythroughHandler(GameNavigation navigation, Score score)
        {
            _navigation = navigation;
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
            _navigation.ToWin();
            GameWinCommand.Execute();
            _disposable?.Clear();
        }

        private void OnGameLost()
        {
            _navigation.ToLost();
            _disposable?.Clear();
        }
    }
}