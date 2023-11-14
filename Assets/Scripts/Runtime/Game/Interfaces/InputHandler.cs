using UniRx;

namespace Core.Game.Input
{
    public abstract class InputHandler
    {
        public readonly ReactiveCommand _holdStartedCommand = new();
        public readonly ReactiveCommand _holdEndedCommand = new();
        public readonly ReactiveCommand _movedLeftCommand = new();
        public readonly ReactiveCommand _stopLeftCommand = new();
        public readonly ReactiveCommand _movedRightCommand = new();
        public readonly ReactiveCommand _stopRightCommand = new();
        public readonly ReactiveCommand _dashedCommand = new();

        public ReactiveCommand HoldStartedCommand => _holdStartedCommand;
        public ReactiveCommand HoldEndedCommand => _holdEndedCommand;
        public ReactiveCommand MovedLeftCommand => _movedLeftCommand;
        public ReactiveCommand StopLeftCommand => _stopLeftCommand;
        public ReactiveCommand MovedRightCommand => _movedRightCommand;
        public ReactiveCommand StopRightCommand => _stopRightCommand;
        public ReactiveCommand DashedCommand => _dashedCommand;
    }
}