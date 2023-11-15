using UnityEngine;

namespace Core.Game.Input
{
    public class SwipeData
    {
        public Vector2 VectorDirection { get; set; }
        public SwipeDirection SwipeDirection { get; set; }
        public float DotProduct { get; set; }

        public SwipeData(Vector2 vectorDirection, SwipeDirection swipeDirection)
        {
            VectorDirection = vectorDirection;
            SwipeDirection = swipeDirection;
            DotProduct = 0f;
        }

        public void CalculateDotProduct(Vector2 direction) =>
            DotProduct = Vector2.Dot(VectorDirection, direction);
    }
}
