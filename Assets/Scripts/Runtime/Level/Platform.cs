using UnityEngine;

namespace Core.Level
{
    public abstract class Platform : MonoBehaviour
    {
        public const float Length = 60f;

        protected void OnDrawGizmos()
        {
            float halfWidth = Length * 0.5f;

            Color gizmosColor = Color.red;

            Gizmos.color = gizmosColor;

            Vector3 center = transform.position;
            Vector3 leftTop = center + Vector3.left * halfWidth + Vector3.up;
            Vector3 leftBottom = center + Vector3.left * halfWidth + Vector3.down;
            Vector3 rightTop = center + Vector3.right * halfWidth + Vector3.up;
            Vector3 rightBottom = center + Vector3.right * halfWidth + Vector3.down;

            Gizmos.DrawLine(leftTop, leftBottom);
            Gizmos.DrawLine(rightTop, rightBottom);
            Gizmos.DrawLine(leftTop, rightTop);
            Gizmos.DrawLine(leftBottom, rightBottom);
        }
    }
}