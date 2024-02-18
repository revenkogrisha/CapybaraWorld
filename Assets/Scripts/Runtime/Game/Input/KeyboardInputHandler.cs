using Core.Editor.Debugger;
using UnityEngine.InputSystem;

namespace Core.Game.Input
{
    public class KeyboardInputHandler : InputHandler
    {
        private readonly KeyInputCollection _collection;

        public KeyboardInputHandler(KeyInputCollection collection) => 
            _collection = collection;

        public override void Initialize()
        {
            RDebug.Log(1);
            _collection.MovementAction.action.performed += OnMovementStarted;
            _collection.MovementAction.action.canceled += OnMovementEnded;

            _collection.DashAction.action.performed += OnDash;
            _collection.JumpAction.action.performed += OnJump;
            _collection.JumpAction.action.canceled += OnJumpEnded;
        }

        public override void Dispose()
        {
            RDebug.Log(2);
            _collection.MovementAction.action.performed -= OnMovementStarted;
            _collection.MovementAction.action.canceled -= OnMovementEnded;

            _collection.DashAction.action.performed -= OnDash;
            _collection.JumpAction.action.performed -= OnJump;
            _collection.JumpAction.action.canceled -= OnJumpEnded;
        }

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            RDebug.Log(3);
            float axis = context.ReadValue<float>();
            if (axis < 0)
                MoveLeftCommand.Execute();
            else if (axis > 0)
                MoveRightCommand.Execute();
        }

        private void OnMovementEnded(InputAction.CallbackContext context)
        {
            StopCommand.Execute();
        }

        private void OnDash(InputAction.CallbackContext context)
        {
            RDebug.Log(4);
            DashCommand.Execute();
        }

        private void OnJump(InputAction.CallbackContext context)
        {
            JumpCommand.Execute();
            HoldStartCommand.Execute();
        }

        private void OnJumpEnded(InputAction.CallbackContext context)
        {
            HoldEndCommand.Execute();
        }
    }
}