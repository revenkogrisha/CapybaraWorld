using System.Collections.Generic;
using UnityEngine;

namespace UnityTools
{
    public static class GameObjectExtensions
    {
        /// <summary>
        /// Returns array with all materials attached to GameObject
        /// </summary
        public static Material[] GetAllMaterials(this GameObject gameObject)
        {
            Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
            var materials = new List<Material>();
            foreach (var renderer in renderers)
                materials.Add(renderer.material);

            return materials.ToArray();
        }

        public static bool HasComponent<T>(this GameObject container) 
            where T : Object =>
            container.GetComponent<T>();

        public static bool CompareLayers(this GameObject container, LayerMask layerMask) =>
            layerMask == (layerMask | (1 << container.layer));
    }
}