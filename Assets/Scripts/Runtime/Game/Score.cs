namespace Core.Game
{
    public class Score
    {
        private int _playthroughScore;
        private int _highestScore;

        public int PlaythroughScore
        {
            get => _playthroughScore;
            set => _playthroughScore = value;
        }

        public int HighestScore => _highestScore;

        public void CaptureHighestScore() =>
            _highestScore = PlaythroughScore;
    }
}
