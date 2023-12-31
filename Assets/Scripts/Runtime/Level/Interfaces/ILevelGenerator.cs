using System;
using Core.UI;
using UnityEngine;

namespace Core.Level
{
    public interface ILevelGenerator : IDisposable
    {
        public void Clean();
        public void Generate();
        public void InitializeLabels(AreaLabelContainer container);
        public void Initialize(Transform center);
        public void SetActiveWorldCanvases(bool state);
    }
}
