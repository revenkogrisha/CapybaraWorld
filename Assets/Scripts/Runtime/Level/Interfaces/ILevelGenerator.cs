using System;
using Core.Player;
using UnityEngine;

namespace Core.Level
{
    public interface ILevelGenerator : IDisposable
    {
        public void Generate();

        public void InitializeCenter(Transform center);
    }
}
