using Core.UI;

namespace Core.Game.Input
{
    public class ButtonsInputHandler : InputHandler
    {
        private ButtonsUI _buttons;
        
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
            _buttons.Left.OnClickStarted -= OnMoveLeft;
            _buttons.Right.OnClickStarted -= OnMoveRight;
            _buttons.Left.OnClickEnded -= OnMovementEnded;
            _buttons.Right.OnClickEnded -= OnMovementEnded;
            _buttons.Descend.OnClickStarted -= OnDescend;
            _buttons.Dash.OnClickStarted -= OnDash;
            _buttons.Jump.OnClickStarted -= OnJump;
            _buttons.Jump.OnClickEnded -= OnJumpEnded;
        }

        public void AssignUI(ButtonsUI buttons) =>
            _buttons = buttons;

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
