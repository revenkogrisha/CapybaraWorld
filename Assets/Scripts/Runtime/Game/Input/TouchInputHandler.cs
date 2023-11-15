using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Game.Input
{
    public class TouchInputHandler : InputHandler, IDisposable
    {
        private readonly TouchInputCollection _collection;
        private readonly SwipeDetector _swipeDetector = new();

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
            HandleSwipeStart();
            ExecuteMove();
        }

        private void OnHoldActionEnded(InputAction.CallbackContext context)
        {
            HoldEndCommand.Execute();
            StopCommand.Execute();
            HandleSwipeEnd();
        }

        private void OnDashAction(InputAction.CallbackContext context) => 
            DashCommand.Execute();

        private void ExecuteMove()
        {
            Vector2 position = GetTouchPosition();
            bool touchIsOnRight = position.x > Screen.width * 0.5f;

            _ = touchIsOnRight == true
                ? MoveRightCommand.Execute()
                : MoveLeftCommand.Execute();
        }

        private void HandleSwipeStart()
        {
            Vector2 position = GetTouchPosition();
            _swipeDetector.HandleTouchStart(position, Time.time);
        }

        private void HandleSwipeEnd()
        {
            Vector2 position = GetTouchPosition();
            SwipeDirection swipe = _swipeDetector.HandleTouchEnd(position, Time.time);
            switch (swipe)
            {
                case SwipeDirection.Up:
                    Debug.Log("Up!");
                    break;

                case SwipeDirection.Down:
                    Debug.Log("Down!");
                    break;

                case SwipeDirection.Unknown:
                    Debug.Log("Unknown!");
                    break;
            }
        }

        private Vector2 GetTouchPosition() =>
            _collection.PositionAction.action.ReadValue<Vector2>();
    }
}