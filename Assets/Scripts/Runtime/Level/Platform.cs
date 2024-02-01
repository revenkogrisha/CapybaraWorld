using System.Diagnostics;
using TriInspector;
using UnityEngine;

namespace Core.Level
{
    [DeclareHorizontalGroup("Buttons")]
    public abstract class Platform : MonoBehaviour
    {
        public const float Length = 120f;

        [Tooltip("Can be null, if not - hides on Start, shows on Gameplay State enter")]
        [SerializeField] private Canvas _worldCanvas;

        [ListDrawerSettings(AlwaysExpanded = true, HideAddButton = true)]
        [Space]
        [SerializeField] private SpawnMarker[] _spawnMarkers;


        public SpawnMarker[] SpawnMarkers => _spawnMarkers;

        #region MonoBehaviour

        [Conditional("UNITY_EDITOR")]
        protected void OnDrawGizmos()
        {
            if (transform == null)
                return;

            const float halfWidth = Length * 0.5f;
            Gizmos.color = Color.red;

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
        
        #endregion

        public Canvas GetWorldCanvasIfHas() =>
            _worldCanvas ?? null;

        [Button(ButtonSizes.Large), GUIColor(255, 153, 153), Group("Buttons")]
        private void ClearMarkers() =>
            _spawnMarkers = null;

        [Button(ButtonSizes.Large), GUIColor(153, 225, 153), Group("Buttons")]
        private void AssignMarkers() =>
            _spawnMarkers = GetComponentsInChildren<SpawnMarker>();
    }
}