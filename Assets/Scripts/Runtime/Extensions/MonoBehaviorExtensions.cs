using UnityEngine;

namespace Core.Other
{
    public static class MonoBehaviorExtensions
    {
        public static void SetPosition(this MonoBehaviour behaviour, Vector2 position) =>
            behaviour.transform.position = position;

        public static void SetPosition(this MonoBehaviour behaviour, Vector3 position) =>
            behaviour.transform.position = position;

        public static void SetLocalPosition(this MonoBehaviour behaviour, Vector3 position) =>
            behaviour.transform.localPosition = position;

        public static Vector3 GetPosition(this MonoBehaviour behaviour) =>
            behaviour.transform.position;

        public static Vector3 GetLocalScale(this MonoBehaviour behaviour) =>
            behaviour.transform.localScale;

        public static Vector3 SetLocalScale(this MonoBehaviour behaviour, Vector3 scale) =>
            behaviour.transform.localScale = scale;

        public static Vector3 GetLocalPosition(this MonoBehaviour behaviour) =>
            behaviour.transform.localPosition;

        public static void SetParent(this MonoBehaviour behaviour, Transform parent) =>
            behaviour.transform.parent = parent;
    }
}
