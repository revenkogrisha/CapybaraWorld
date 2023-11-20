using System;
using UnityEngine;

namespace Core.Level
{
    public interface ILevelGenerator : IDisposable
    {
        public void Generate();
        public void InitializeCenter(Transform center);
        public void Clean();
    }
}
