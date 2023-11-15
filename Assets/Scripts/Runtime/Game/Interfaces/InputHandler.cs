using System;
using UniRx;

namespace Core.Game.Input
{
    public abstract class InputHandler : IDisposable
    {
        private readonly ReactiveCommand _holdStartCommand = new();
        private readonly ReactiveCommand _holdEndCommand = new();
        private readonly ReactiveCommand _moveLeftCommand = new();
        private readonly ReactiveCommand _moveRightCommand = new();
        private readonly ReactiveCommand _stopCommand = new();
        private readonly ReactiveCommand _dashCommand = new();

        public ReactiveCommand HoldStartCommand => _holdStartCommand;
        public ReactiveCommand HoldEndCommand => _holdEndCommand;
        public ReactiveCommand MoveLeftCommand => _moveLeftCommand;
        public ReactiveCommand StopCommand => _stopCommand;
        public ReactiveCommand MoveRightCommand => _moveRightCommand;
        public ReactiveCommand DashCommand => _dashCommand;

        public abstract void Initialize();

        public abstract void Dispose();
    }
}