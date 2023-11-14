using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Game.Input
{
    public class TouchInputHandler : InputHandler, IDisposable
    {
        private readonly TouchInputCollection _collection;
        private bool _wasLastTouchOnTheRight;

        public TouchInputHandler(TouchInputCollection collection) =>
            _collection = collection;

        public override void Initialize()
        {
            _collection.HoldAction.action.started += _ => HoldStartCommand.Execute();
            _collection.HoldAction.action.canceled += _ => HoldEndCommand.Execute();

            _collection.HoldAction.action.started += _ => OnPositionHoldActionStarted();
            _collection.HoldAction.action.canceled += _ => OnPositionHoldActionEnded();

            _collection.DashAction.action.performed += _ => DashCommand.Execute();
        }

        public override void Dispose()
        {
            _collection.HoldAction.action.performed -= _ => HoldStartCommand.Execute();
            _collection.HoldAction.action.canceled -= _ => HoldEndCommand.Execute();

            _collection.HoldAction.action.started -= _ => OnPositionHoldActionStarted();
            _collection.HoldAction.action.canceled -= _ => OnPositionHoldActionEnded();

            _collection.DashAction.action.performed -= _ => DashCommand.Execute();
        }

        private void OnPositionHoldActionStarted()
        {
            Vector2 touchPosition = _collection.PositionAction.action.ReadValue<Vector2>();
            bool touchIsOnRight = touchPosition.x > Screen.width * 0.5f;
            _wasLastTouchOnTheRight = touchIsOnRight;

            _ = _wasLastTouchOnTheRight == true
                ? MoveRightCommand.Execute()
                : MoveLeftCommand.Execute();
        }

        private void OnPositionHoldActionEnded()
        {
            _ = _wasLastTouchOnTheRight == true
                ? StopRightCommand.Execute()
                : StopLeftCommand.Execute();
        }
    }
}