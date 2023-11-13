using UniRx;

namespace Core.Game
{
    public class Score
    {
        public readonly ReactiveProperty<int> PlaythroughScore = new();
        
        private int _highestScore;

        public int HighestScore => _highestScore;

        public void CaptureHighestScore() =>
            _highestScore = PlaythroughScore.Value;
    }
}