using System;
using UnityEngine;

namespace Core.Other
{
    [Serializable]
    public class VectorRange
    {
        [SerializeField] private Vector2 _minimum;
        [SerializeField] private Vector2 _maximum;

        public Vector2 Minimum => _minimum;
        public Vector2 Maximum => _maximum;

        public VectorRange(Vector2 minimum, Vector2 maximum)
        {
            _minimum = minimum;
            _maximum = maximum;
        }
    }
}
