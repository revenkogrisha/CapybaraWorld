using UnityEngine;

namespace Core.Other
{
    public static class MonoBehaviorExtensions
    {
        public static void SetPosition(this MonoBehaviour behaviour, Vector2 position) =>
            behaviour.transform.position = position;

        public static void SetPosition(this MonoBehaviour behaviour, Vector3 position) =>
            behaviour.transform.position = position;

        public static Vector3 GetPosition(this MonoBehaviour behaviour) =>
            behaviour.transform.position;

        public static void SetParent(this MonoBehaviour behaviour, Transform parent) =>
            behaviour.transform.parent = parent;
    }
}
