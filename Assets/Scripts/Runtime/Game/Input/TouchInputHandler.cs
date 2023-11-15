using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Game.Input
{
    public class TouchInputHandler : InputHandler, IDisposable
    {
        private readonly TouchInputCollection _collection;

        public TouchInputHandler(TouchInputCollection collection) =>
            _collection = collection;

        public override void Initialize()
        {
            _collection.HoldAction.action.started += OnHoldActionStarted;
            _collection.HoldAction.action.canceled += OnHoldActionEnded;
            _collection.DashAction.action.performed += OnDashAction;
        }

        public override void Dispose()
        {
            _collection.HoldAction.action.started -= OnHoldActionStarted;
            _collection.HoldAction.action.canceled -= OnHoldActionEnded;
            _collection.DashAction.action.performed -= OnDashAction;
        }

        private void OnHoldActionStarted(InputAction.CallbackContext context)
        {
            HoldStartCommand.Execute();
            ExecuteMove();
        }

        private void OnHoldActionEnded(InputAction.CallbackContext context)
        {
            HoldEndCommand.Execute();
            StopCommand.Execute();
        }

        private void OnDashAction(InputAction.CallbackContext context) => 
            DashCommand.Execute();

        private void ExecuteMove()
        {
            Vector2 touchPosition = _collection.PositionAction.action.ReadValue<Vector2>();
            bool touchIsOnRight = touchPosition.x > Screen.width * 0.5f;

            _ = touchIsOnRight == true
                ? MoveRightCommand.Execute()
                : MoveLeftCommand.Execute();
        }
    }
}