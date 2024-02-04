using UnityEngine;

namespace Core.Other
{
    public static class MonoBehaviorExtensions
    {
        public static void SetPosition(this Behaviour behaviour, Vector2 position) =>
            behaviour.transform.position = position;

        public static void SetPosition(this Behaviour behaviour, Vector3 position) =>
            behaviour.transform.position = position;

        public static void SetLocalPosition(this Behaviour behaviour, Vector3 position) =>
            behaviour.transform.localPosition = position;

        public static Vector3 GetPosition(this Behaviour behaviour) =>
            behaviour.transform.position;

        public static Vector3 GetLocalScale(this Behaviour behaviour) =>
            behaviour.transform.localScale;

        public static Vector3 SetLocalScale(this Behaviour behaviour, Vector3 scale) =>
            behaviour.transform.localScale = scale;

        public static Vector3 GetLocalPosition(this Behaviour behaviour) =>
            behaviour.transform.localPosition;

        public static void SetParent(this Behaviour behaviour, Transform parent) =>
            behaviour.transform.parent = parent;

        public static void SetActive(this Behaviour behaviour, bool value) =>
            behaviour.gameObject.SetActive(value);
    }
}
