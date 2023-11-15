using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core.Game.Input
{
    public class SwipeDetector
    {
        private const float MaximumTime = 1f;
        private const float MinimumDistance = 1f;
        private const float DirectionDotThreshold = 0.85f;

        private readonly Dictionary<Vector2, SwipeDirection> _directions = new();
        private Vector2 _startPosition;
        private Vector2 _endPosition;
        private float _startTime;
        private float _endTime;

        public SwipeDetector()
        {
            _directions.Add(Vector2.up, SwipeDirection.Up);
            _directions.Add(Vector2.down, SwipeDirection.Down);
            _directions.Add(Vector2.left, SwipeDirection.Left);
            _directions.Add(Vector2.right, SwipeDirection.Right);
        }

        public void HandleTouchStart(Vector2 position, float time)
        {
            _startPosition = position;
            _startTime = time;
        }

        public SwipeDirection HandleTouchEnd(Vector2 position, float time)
        {
            _endPosition = position;
            _endTime = time;

            float swipeTime = _endTime - _startTime;
            float distance = Vector2.Distance(_endPosition, _startPosition);
            if (distance < MinimumDistance 
                || swipeTime > MaximumTime)
                return SwipeDirection.Unknown;

            Vector2 direction = (_endPosition - _startPosition).normalized;
            return DetectDirection(direction);
        }

        private SwipeDirection DetectDirection(Vector2 direction)
        {
            if (DirectionDotThreshold > 1f || DirectionDotThreshold < 0f)
                throw new ArgumentOutOfRangeException(
                    $"Vector Direction Threshold should be in 0f - 1f range! {DirectionDotThreshold}");

            SwipeData[] swipes = GetMajorDirectionsSwipeDatas();
            foreach (SwipeData swipe in swipes)
                swipe.CalculateDotProduct(direction);

            SwipeData currentSwipe = swipes.FirstOrDefault(
                swipe => swipe.DotProduct > DirectionDotThreshold);

            if (currentSwipe == null)
                return SwipeDirection.Unknown;
            else
                return currentSwipe.SwipeDirection;
        }

        private SwipeData[] GetMajorDirectionsSwipeDatas()
        {
            return new SwipeData[]
            {
                CreateSwipeData(Vector2.up),
                CreateSwipeData(Vector2.down),
                CreateSwipeData(Vector2.left),
                CreateSwipeData(Vector2.right)
            };
        }

        private SwipeData CreateSwipeData(Vector2 vector)
        {
            if (_directions.ContainsKey(vector) == false)
                throw new ArgumentException(
                    $"No such Vector2 key in directions dictionary: {vector}");

            return new(vector, _directions[vector]);
        }
    }
}
