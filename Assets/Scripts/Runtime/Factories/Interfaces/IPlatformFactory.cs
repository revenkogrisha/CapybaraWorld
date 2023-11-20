using Core.Level;
using UnityEngine;

namespace Core.Factories
{
    public interface IPlatformFactory<TProduct> where TProduct : Platform
    {
        public TProduct CreateSimple(Vector2 position);
        public TProduct CreateSpecial(Vector2 position);
        public TProduct Create(TProduct prefab);
    }
}
