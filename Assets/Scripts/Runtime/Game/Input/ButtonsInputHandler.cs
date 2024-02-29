using Core.Factories;
using Core.Other;
using Core.UI;

namespace Core.Game.Input
{
    public class ButtonsInputHandler : InputHandler, IInputUIHandler
    {
        private readonly UIProvider _uiProvider;
        private ButtonsUI _buttons;

        public ButtonsInputHandler(UIProvider uiProvider) =>
            _uiProvider = uiProvider;

        public override void Initialize()
        {
            _buttons.Left.OnClickStarted += OnMoveLeft;
            _buttons.Right.OnClickStarted += OnMoveRight;
            _buttons.Left.OnClickEnded += OnMovementEnded;
            _buttons.Right.OnClickEnded += OnMovementEnded;
            _buttons.Descend.OnClickStarted += OnDescend;
            _buttons.Dash.OnClickStarted += OnDash;
            _buttons.Jump.OnClickStarted += OnJump;
            _buttons.Jump.OnClickEnded += OnJumpEnded;
        }

        public override void Dispose()
        {
            _buttons.gameObject.SelfDestroy();

            _buttons.Left.OnClickStarted -= OnMoveLeft;
            _buttons.Right.OnClickStarted -= OnMoveRight;
            _buttons.Left.OnClickEnded -= OnMovementEnded;
            _buttons.Right.OnClickEnded -= OnMovementEnded;
            _buttons.Descend.OnClickStarted -= OnDescend;
            _buttons.Dash.OnClickStarted -= OnDash;
            _buttons.Jump.OnClickStarted -= OnJump;
            _buttons.Jump.OnClickEnded -= OnJumpEnded;
        }

        public void CreateUI() =>
            _buttons = _uiProvider.CreateButtonsUI();

        private void OnMoveLeft() => 
            MoveLeftCommand.Execute();

        private void OnMoveRight() => 
            MoveRightCommand.Execute();

        private void OnMovementEnded() => 
            StopCommand.Execute();

        private void OnDescend() => 
            DownCommand.Execute();

        private void OnDash() => 
            DashCommand.Execute();

        private void OnJump()
        {
            JumpCommand.Execute();
            HoldStartCommand.Execute();
        }

        private void OnJumpEnded() => 
            HoldEndCommand.Execute();
    }
}