using UnityEngine;

namespace Core.Other
{
    public static class CollisionsExtensions
    {
        public static bool CompareLayers(this Collision container, LayerMask layerMask) =>
            container.gameObject.CompareLayers(layerMask);

        public static bool CompareLayers(this Collision2D container, LayerMask layerMask) =>
            container.gameObject.CompareLayers(layerMask);
    }
}
