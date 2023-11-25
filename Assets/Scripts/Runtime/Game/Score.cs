using UniRx;

namespace Core.Game
{
    public class Score
    {
        public readonly ReactiveProperty<int> PlaythroughScore = new();
    }
}