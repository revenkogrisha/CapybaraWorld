using NTC.Pool;
using UnityEngine;

namespace Core.Other
{
    public static class GameObjectExtensions
    {
        public static bool HasComponent<T>(this GameObject container) 
            where T : Object =>
            container.GetComponent<T>();

        public static bool CompareLayers(this GameObject container, LayerMask layerMask) =>
            layerMask == (layerMask | (1 << container.layer));

        public static void SelfDestroy(this GameObject gameObject) =>
            Object.Destroy(gameObject);

        public static void SelfDespawn(this GameObject gameObject) =>
            NightPool.Despawn(gameObject);

        public static void SetPosition(this GameObject gameObject, Vector3 position) =>
            gameObject.transform.position = position;

        public static void SetPosition(this GameObject gameObject, Vector2 position) =>
            gameObject.transform.position = position;
    }
}