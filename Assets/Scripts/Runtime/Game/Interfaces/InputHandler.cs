using System;
using UniRx;

namespace Core.Game.Input
{
    public abstract class InputHandler : IDisposable
    {
        public readonly ReactiveCommand _holdStartedCommand = new();
        public readonly ReactiveCommand _holdEndedCommand = new();
        public readonly ReactiveCommand _movedLeftCommand = new();
        public readonly ReactiveCommand _stopLeftCommand = new();
        public readonly ReactiveCommand _movedRightCommand = new();
        public readonly ReactiveCommand _stopRightCommand = new();
        public readonly ReactiveCommand _dashedCommand = new();

        public ReactiveCommand HoldStartCommand => _holdStartedCommand;
        public ReactiveCommand HoldEndCommand => _holdEndedCommand;
        public ReactiveCommand MoveLeftCommand => _movedLeftCommand;
        public ReactiveCommand StopLeftCommand => _stopLeftCommand;
        public ReactiveCommand MoveRightCommand => _movedRightCommand;
        public ReactiveCommand StopRightCommand => _stopRightCommand;
        public ReactiveCommand DashCommand => _dashedCommand;

        public abstract void Initialize();

        public abstract void Dispose();
    }
}