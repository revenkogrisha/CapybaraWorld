using System.Diagnostics;
using TriInspector;
using UnityEngine;

namespace Core.Level
{
    [DeclareHorizontalGroup("Buttons")]
    public abstract class Platform : MonoBehaviour
    {
        public const float Length = 120f;

        [ListDrawerSettings(AlwaysExpanded = true, HideAddButton = true)]
        [SerializeField] private SpawnMarker[] _spawnMarkers;

        public SpawnMarker[] SpawnMarkers => _spawnMarkers;

        [Conditional("UNITY_EDITOR")]
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

        [Button(ButtonSizes.Large), GUIColor(255, 153, 153), Group("Buttons")]
        private void ClearMarkers() =>
            _spawnMarkers = null;

        [Button(ButtonSizes.Large), GUIColor(153, 225, 153), Group("Buttons")]
        private void AssignMarkers() =>
            _spawnMarkers = GetComponentsInChildren<SpawnMarker>();
    }
}